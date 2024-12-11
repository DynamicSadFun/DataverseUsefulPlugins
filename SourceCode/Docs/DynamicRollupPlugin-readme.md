# Dynamic Parent-Child Rollup Calculation Plugin

## 📄 Overview

The **Dynamic Parent-Child Rollup Calculation** plugin enables real-time aggregation of values from child records to parent records in Microsoft Dataverse. It is highly configurable and supports various aggregation types like **Sum**, **Count**, and **Average**.

## Features

- **Real-Time Aggregation**:
  Automatically updates parent record fields based on changes in child records.
  
- **Dynamic Configuration**:
  Use a custom table (`Rollup Configuration`) to define the parent-child relationship, source fields, target fields, and aggregation type.

- **Customizable Aggregations**:
  Supports **Sum**, **Count**, and **Average** calculations.

- **Error Handling**:
  Logs errors for debugging and tracking purposes.

---

## Prerequisites

1. A custom table named `Rollup Configuration` in Dataverse with the following fields:
   - `Parent Entity` (Text): Logical name of the parent entity.
   - `Child Entity` (Text): Logical name of the child entity.
   - `Source Field` (Text): Field in the child entity to aggregate.
   - `Target Field` (Text): Field in the parent entity to store the aggregated value.
   - `Aggregation Type` (Text): Type of aggregation (`Sum`, `Count`, `Average`).

2. Parent and child entities must have a relationship for the plugin to work.

---

## How to Use

1. **Register the Plugin**:
   - Use the **Plugin Registration Tool** to register the `DynamicRollupPlugin` assembly.
   - Register the plugin on `Create`, `Update`, and `Delete` messages for the child entity.

2. **Configure the Rollup Logic**:
   - Add a record in the `Rollup Configuration` table specifying the parent-child relationship, source field, target field, and aggregation type.

3. **Test the Plugin**:
   - Create, update, or delete child records to see the real-time aggregation on the parent record.

---

## Example Configuration

| Parent Entity | Child Entity     | Source Field | Target Field   | Aggregation Type |
|---------------|------------------|--------------|----------------|------------------|
| `account`     | `invoice`        | `totalamount`| `totalinvoices`| `Sum`            |
| `project`     | `task`           | `taskstatus` | `completedtasks`| `Count`         |

---

## Known Limitations

- Does not support complex aggregation logic involving multiple fields.
- Large datasets may impact performance; consider batch updates for high-volume scenarios.

---

## Troubleshooting

- **Plugin Errors**:
  Check the Dataverse plugin trace log for error details.

- **Incorrect Calculations**:
  Verify the `Rollup Configuration` table for correct mappings.
