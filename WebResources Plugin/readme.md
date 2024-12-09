# WebResources Plugin

## 📄 Overview

The **WebResources Plugin** is a **Pre-Operation Dataverse plugin** designed to streamline and enhance the behavior of the `webresource` entity. It ensures consistency and automation for managing web resource extensions during record creation or update processes.

### 🎯 Key Features

- Automatically appends the correct file extension to the **Logical Name** (`name`) and **Display Name** (`displayname`) of a web resource, based on its type.
- Prevents duplicate extensions by validating the current values.
- Enforces a clean and standardized naming convention for web resources, reducing manual errors.

---

## 🛠️ Technical Details

### **Trigger Point**
- **Entity**: `webresource`
- **Message**: `Create`, `Update`
- **Stage**: **Pre-Operation**

### **Logic Workflow**
1. **Input Validation**: Checks if the `name` attribute (Logical Name) and `webresourcetype` attribute are present in the `Target` entity.
2. **Determine Extension**: Extracts the appropriate file extension for the web resource using the `webresourcetype` attribute.
3. **Extension Check**: If the extension is missing or incorrect, it appends the correct extension to the Logical Name and Display Name.
4. **Error Handling**: Throws a descriptive error if the extension cannot be determined or other required attributes are missing.

---

## 🔧 How to Use

### 1. **Registering the Plugin**
- Use the **Plugin Registration Tool** to register this plugin with the following configurations:
  - **Entity**: `webresource`
  - **Message**: `Create`, `Update`
  - **Stage**: `Pre-Operation`
  - **Execution Mode**: `Synchronous`
  
### 2. **Customization Scenarios**
- Automatically handles extensions such as `.js`, `.html`, `.css`, etc., based on the `webresourcetype`.
- Reduces manual errors when managing web resources during solution development.

---

## 📂 Folder Structure

```plaintext
WebResources Plugin/
│
├── WebResourcesAddExtension.cs      # Main plugin logic
├── Constants.cs                     # Contains attribute and entity constants
├── README.md                        # Documentation for this plugin
└── Tests/                           # Unit tests for plugin validation
```

---

## 💻 Example Code

The plugin is implemented in **C#** and uses the Dataverse SDK to interact with the platform. Here's a brief snippet of the logic:

```csharp
if (targetEntity.Attributes.Contains("name") && 
    targetEntity.Attributes.Contains("webresourcetype"))
{
    var logicalName = targetEntity.GetAttributeValue<string>("name");
    var webResourceType = targetEntity.GetAttributeValue<OptionSetValue>("webresourcetype")?.Value;

    // Determine file extension based on web resource type
    string extension = WebResourceHelper.GetExtensionByType(webResourceType);
    if (!string.IsNullOrEmpty(extension) && !logicalName.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
    {
        // Update logical name and display name with the correct extension
        targetEntity["name"] = $"{logicalName}.{extension}";
        if (targetEntity.Attributes.Contains("displayname"))
        {
            targetEntity["displayname"] = $"{logicalName}.{extension}";
        }
    }
}
```

---

## 🧪 Testing

1. **Unit Tests**: Validate behavior under various scenarios (missing attributes, correct extension, etc.).
2. **Test Cases**:
   - Input `webresourcetype = 1` (JavaScript), `name = "myScript"` → Output: `name = "myScript.js"`.
   - Input `webresourcetype = 2` (HTML), `name = "page"` → Output: `name = "page.html"`.

---

## 🚨 Error Handling

- **Missing Attributes**: Throws an exception if `name` or `webresourcetype` is not provided.
- **Invalid Web Resource Type**: Returns a descriptive error if the type is unknown or unsupported.

---

## 🛡️ License

This plugin follows the repository's **MIT License**.

---

Let me know if you need any additional information or tweaks!
