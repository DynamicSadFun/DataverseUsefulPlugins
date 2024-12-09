# Dataverse Useful Plugins

This repository contains a collection of **useful plugins for Microsoft Dataverse**, tailored to address common development challenges and extend Dataverse functionality with custom business logic. Each plugin is built with robust practices and optimized for performance in Dataverse environments.

## 🚀 Technology Stack

- **Language**: C#  
- **Framework**: .NET Framework (typically targeting 4.x versions compatible with Dataverse)  
- **Dataverse SDK**: Microsoft Dataverse SDK for plugin development  
- **Development Tools**: Visual Studio, Plugin Registration Tool (XrmToolBox optional)

## 📁 Repository Structure

```plaintext
DataverseUsefulPlugins/
│
├── src/                # Source code for plugins
│   ├── PluginName1/
│   ├── PluginName2/
│   └── SharedLibrary/  # Shared utilities, constants, or helper functions
│
├── tests/              # Unit tests for plugins
│
├── docs/               # Documentation, usage guides, and technical details
│
├── PluginRegistration/ # Sample JSONs or scripts for Plugin Registration Tool
│
├── LICENSE             # License file
├── README.md           # Repository overview
└── CONTRIBUTING.md     # Contribution guidelines (if public repo)
```

## 🔧 Prerequisites

- **Microsoft Dataverse Environment**: A valid environment to register and test plugins.
- **Plugin Registration Tool**: To register your plugin assemblies and steps.
- **Visual Studio**: For building and debugging the plugin code.
- **NuGet Packages**: 
  - Microsoft.CrmSdk.CoreAssemblies
  - Microsoft.CrmSdk.XrmTooling.CoreAssembly

## 🛠️ How to Use

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/DynamicSadFun/DataverseUsefulPlugins.git
   ```

2. **Build the Solution**:
   Open the solution in Visual Studio, restore NuGet packages, and build the project to generate the plugin assembly.

3. **Register the Plugin**:
   - Use the **Plugin Registration Tool** to register the plugin assembly in your Dataverse environment.
   - Create steps with appropriate triggers (e.g., Pre-Operation, Post-Operation) on target entities and messages.

4. **Deploy**:
   - Import the plugin assembly into your target Dataverse environment.
   - Test the plugin to ensure functionality.

## 🌟 Plugin Highlights

Each plugin in this repository is crafted to solve specific use cases:

1. **PluginName1**:  
   _Description_: Implements logic to validate entity fields before the record is created.  
   _Usage_: Pre-Operation plugin for the `Create` message.

2. **PluginName2**:  
   _Description_: Automatically populates a related field when a record is updated.  
   _Usage_: Post-Operation plugin for the `Update` message.

3. **SharedLibrary**:  
   _Description_: Contains reusable helper methods and constants for Dataverse operations.  

## 📖 Documentation

- **Detailed Instructions**: Available in the `/docs` folder.
- **Configuration Files**: Samples for plugin steps and secure configuration are in `/PluginRegistration`.

## 🛡️ License

This project is licensed under the **MIT License**. See the `LICENSE` file for details.

