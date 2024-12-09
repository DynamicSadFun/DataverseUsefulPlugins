### **Plugin: Audit Activity for Critical Fields**

#### **Description**

This plugin logs changes made to specific critical fields in any entity and stores the change history in a custom `AuditLog` table. This is useful for tracking updates to sensitive data, providing a more detailed audit trail than the out-of-the-box Dataverse audit functionality.

---

### **Key Features**

1. **Universal Applicability**:
   - Works across any entity in Dataverse.
   - Configurable to track specific fields (e.g., `emailaddress`, `statuscode`, `priority`, etc.).
   
2. **Detailed Change Logging**:
   - Logs old and new values of fields.
   - Captures information such as `ModifiedBy`, `ModifiedOn`, and the triggering event.

3. **Customizable Configuration**:
   - Critical fields and entities to monitor can be defined in a centralized configuration table.

4. **Optimized Performance**:
   - Runs on the Pre-Operation stage to minimize system load.
   - Logs only significant changes to reduce noise.

---

### **Technical Details**

#### **Trigger Points**
- **Entities**: Configurable (e.g., `contact`, `account`, `custom_entities`).
- **Messages**: `Update`, `Delete`.
- **Stage**: `Pre-Operation` or `Post-Operation`.

#### **Logic Workflow**
1. **Check Entity and Fields**:
   - Verify if the entity and fields are configured for auditing.
   
2. **Compare Values**:
   - Compare the old and new values of configured fields.

3. **Log Changes**:
   - If there’s a change, create a record in the `AuditLog` table with:
     - Entity name, record ID, field name, old value, new value, `ModifiedBy`, `ModifiedOn`.

---

### **Custom Table: AuditLog**

| Column Name        | Data Type    | Description                             |
|--------------------|--------------|-----------------------------------------|
| **Name**           | Text         | Summary of the change (e.g., field name). |
| **OldValue**       | Text         | Previous value of the field.           |
| **NewValue**       | Text         | Updated value of the field.            |
| **ModifiedBy**     | Lookup (User)| The user who made the change.          |
| **ModifiedOn**     | DateTime     | Timestamp of the change.               |
| **EntityName**     | Text         | Name of the entity being tracked.      |
| **RecordId**       | Text         | Unique identifier of the record.       |

---

### **Configuration Table: AuditConfig**

| Column Name        | Data Type    | Description                             |
|--------------------|--------------|-----------------------------------------|
| **EntityName**     | Text         | Name of the entity to audit.           |
| **FieldName**      | Text         | Name of the field to track.            |

---

### **Example Use Cases**

1. **Tracking Status Changes**:
   - Monitor changes to the `statuscode` field in critical entities like `case` or `incident`.

2. **Sensitive Field Monitoring**:
   - Audit updates to the `emailaddress` field in the `contact` table.

3. **Custom Entity Auditing**:
   - Record changes to financial fields like `budget` or `cost` in a custom entity.

---

### **Advantages**
- Provides a detailed and configurable audit trail.
- Helps meet compliance requirements for sensitive data.
- Enables teams to quickly identify and troubleshoot unexpected changes.


### **Key Differences**

| Feature/Functionality      | **OOB Audit**                                                                 | **Custom Audit Activity Plugin**                                         |
|----------------------------|------------------------------------------------------------------------------|--------------------------------------------------------------------------|
| **Customization**          | Limited customization of what to audit (e.g., field-level changes).          | Fully customizable. You can define specific entities and fields for auditing in a configuration table. |
| **Audit Scope**            | Audits almost everything: changes to all fields, operations, and metadata.   | Targets only specific fields/entities defined in the configuration.      |
| **Change Log Granularity** | Logs every update, including unchanged fields (e.g., noisy logs).             | Logs only meaningful changes to specific fields (reduces noise).         |
| **Storage**                | Stores audit logs in a system-managed audit table (not easily queryable).    | Stores logs in a custom `AuditLog` table (easy to query and report on).  |
| **Retention**              | Retention is governed by the environment settings and may auto-purge.         | Retention is fully customizable (e.g., archive or export to a Data Lake).|
| **Integration**            | Not easily integrated into business processes or workflows.                  | Logs are stored in a standard table, making it easy to use in workflows, Power BI, or Power Apps. |
| **Performance Impact**     | May impact performance for large-scale auditing (many fields/entities).       | More efficient as it focuses only on critical fields/entities.           |
| **Extensibility**          | Cannot trigger custom logic based on audit logs.                             | Can trigger workflows or custom business logic based on field changes.   |
| **Ease of Reporting**      | Requires complex queries to extract audit data.                              | Data stored in standard table format, easy to query and visualize.       |

---

### **Use Case Scenarios Where the Plugin Shines**

1. **Targeted Field Auditing**:
   - You only want to track changes to critical fields (e.g., `statuscode`, `email`, `budget`) to reduce the volume of unnecessary logs.
   - Example: Track when a contact's email address is updated but ignore other field changes.

2. **Customizable Reporting**:
   - You need to generate detailed audit reports for specific fields/entities using Power BI or Excel without extracting logs from the OOB audit system.

3. **Integration with Workflows/Processes**:
   - You want to trigger workflows or notifications when a specific field is updated.
   - Example: Send an alert to a manager when the `priority` field on a case is downgraded.

4. **Retention Management**:
   - You want full control over how long audit data is retained and easily export it to a Data Lake or external system.

5. **Compliance Requirements**:
   - You need detailed, configurable audit trails to meet specific industry regulations.
   - Example: A financial organization may need to track specific monetary fields and have full control over log retention.

---

### **Why Use the Plugin Over OOB Audit?**

- **Focused Logging**: By targeting specific fields/entities, the plugin reduces the "noise" in audit logs, making it easier to extract meaningful insights.
- **Query-Friendly Data**: Audit data is stored in a standard table, making it accessible for querying or integrating with reporting tools.
- **Custom Logic**: The plugin lets you trigger workflows or other business processes directly tied to audit changes.
- **Extensibility**: It can be extended to include additional logic, such as validation, notifications, or integrations with external systems.

To implement a **Custom Audit** using the `CustomAudit` plugin as provided in the code, you need a clear design and documentation for both the infrastructure and the functionality. Below is a comprehensive step-by-step guide:

---

# HOW TO USE? 

### **Infrastructure Requirements**
To implement the Custom Audit functionality, you need:

#### **1. Custom Tables in Dataverse**
You need one or more tables to store audit data. Here's an example:

1. **Table Name**: `CustomAuditLog`  
   **Purpose**: To store the details of audited changes.
   - **Columns**:
     | Column Name          | Data Type       | Description                             |
     |----------------------|-----------------|-----------------------------------------|
     | `customauditlogid`   | GUID (Primary)  | Unique identifier for the audit log.   |
     | `entityname`         | String          | Name of the entity being audited.      |
     | `recordid`           | GUID            | ID of the record being audited.        |
     | `fieldname`          | String          | Name of the field that changed.        |
     | `oldvalue`           | String          | The previous value of the field.       |
     | `newvalue`           | String          | The updated value of the field.        |
     | `modifiedby`         | Lookup (User)   | User who made the change.              |
     | `modifiedon`         | DateTime        | Timestamp of the change.               |
     | `operationtype`      | Option Set      | Type of operation (Create/Update/Delete). |
     | `customauditconfigid`| Lookup          | Reference to the audit configuration (optional). |

---

#### **2. Configuration Table**
You can add a configuration table to define which entities and fields should be audited.

1. **Table Name**: `CustomAuditConfiguration`
   **Purpose**: To define the audit scope.
   - **Columns**:
     | Column Name         | Data Type       | Description                              |
     |---------------------|-----------------|------------------------------------------|
     | `customauditconfigid` | GUID (Primary) | Unique identifier for the configuration.|
     | `entityname`        | String          | Name of the entity to audit.            |
     | `fieldname`         | String          | Field to audit (optional).              |
     | `operationtype`     | Option Set      | Operation type to audit (Create/Update/Delete). |

---

### **Plugin Code Workflow**

#### **Step-by-Step Execution**
1. **Trigger**:
   - Register the plugin for the `Create`, `Update`, and `Delete` events.
   - Target: Entities you wish to audit.
   - Execution Stage: **Pre-Operation** or **Post-Operation** depending on requirements.

2. **Audit Configuration**:
   - Use the `GetAuditConfiguration` method to fetch the list of entities and fields to audit from `CustomAuditConfiguration`.

3. **Field Change Detection**:
   - For `Update` events, compare `Pre-Image` (old values) and `Target` (new values).
   - Identify the fields that have changed.

4. **Data Storage**:
   - For every audited field, create a record in the `CustomAuditLog` table with:
     - Entity name
     - Record ID
     - Field name
     - Old value
     - New value
     - User who made the change
     - Timestamp of the change

5. **Additional Features**:
   - Optionally, implement filtering by user roles, entity-specific conditions, or operation type.

---

### **Plugin Registration**

#### **Steps**:
1. **Register the Assembly**:
   - Use the **Plugin Registration Tool** to register the compiled plugin assembly.

2. **Register Steps**:
   - Create steps for each entity and message (Create/Update/Delete).
   - Enable `Pre-Image` for `Update` steps to capture old values.

3. **Images**:
   - **Pre-Image**: For `Update` events, to retrieve old values.
   - **Post-Image**: For `Create` events, to retrieve newly created values.

---

### **CustomAudit Plugin Code Walkthrough**

#### Key Methods
1. **ExecuteDataversePlugin**:
   - Core execution logic to identify changes and store audit logs.

2. **GetAuditConfiguration**:
   - Retrieves the audit configuration from `CustomAuditConfiguration` to determine the audit scope.

3. **CreateAuditLog**:
   - Saves audit records to the `CustomAuditLog` table.

4. **DetectFieldChanges**:
   - Compares `Pre-Image` and `Target` to identify field-level changes.

---

### **Deployment Checklist**

#### **1. Custom Tables**:
- Ensure `CustomAuditLog` and `CustomAuditConfiguration` tables are created and deployed to the environment.

#### **2. Plugin Assembly**:
- Compile the plugin code into a DLL and register it in the target environment.

#### **3. Audit Configuration**:
- Populate `CustomAuditConfiguration` with the required entities, fields, and operation types.

#### **4. Plugin Steps**:
- Register steps for all entities to be audited and ensure correct images are configured.

---

### **Example Use Case**

#### Scenario:
Auditing changes to the `Account` entity, specifically for:
- Field `accountnumber` (Track all changes).
- Operations: Create, Update, and Delete.

#### Configuration:
- Add a record to `CustomAuditConfiguration`:
  - Entity: `Account`
  - Field: `accountnumber`
  - Operations: Update

#### Result:
- When a user updates the `accountnumber` field, the plugin creates a record in `CustomAuditLog` with:
  - The old and new values of `accountnumber`.
  - The user who made the change.
  - The timestamp.

---

### **Best Practices**
1. **Use Pre/Post Images Judiciously**:
   - Only enable images for steps where they are necessary, to minimize performance overhead.

2. **Validate Configuration**:
   - Ensure the `CustomAuditConfiguration` table is populated correctly to avoid unnecessary execution.

3. **Error Handling**:
   - Implement proper exception handling to avoid breaking operations.

4. **Performance Monitoring**:
   - Regularly monitor plugin execution time and optimize fetch operations.

5. **Data Retention**:
   - Set retention policies for `CustomAuditLog` to manage storage.

