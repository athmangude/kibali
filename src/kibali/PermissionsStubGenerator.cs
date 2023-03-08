﻿namespace Kibali;

public class PermissionsStubGenerator
{
    private readonly PermissionsDocument document;
    private readonly string path;
    private readonly string method;
    public PermissionsStubGenerator(PermissionsDocument document, string path, string method)
    {
        this.document = document;
        this.path = path;
        this.method = method;
    }

    public PermissionsDocument Document { get; set; }

    public string Path { get; set; }

    public string Method { get; set; }

    public string GenerateTable()
    {
        var authZChecker = new AuthZChecker();
        authZChecker.Load(this.document);

        var resource = authZChecker.FindResource(this.path);
        var table = this.UnsupportedPermissionsStub();
        if (resource == null)
        {
            return table;
        }
        
        if (!string.IsNullOrEmpty(this.method))
        {
            if (resource.SupportedMethods.TryGetValue(this.method, out var supportedSchemes))
            {
                table = resource.GeneratePermissionsTable(supportedSchemes);
            }
        }
        return table;
    }

    private string UnsupportedPermissionsStub(){
        var permissionsStub = "Not supported.";
        var markdownBuilder = new MarkDownBuilder();
        markdownBuilder.StartTable("Permission type", "Least privileged permission", "Higher privileged permissions");

        markdownBuilder.AddTableRow("Delegated (work or school account)", permissionsStub, permissionsStub);

        markdownBuilder.AddTableRow("Delegated (personal Microsoft account)", permissionsStub, permissionsStub);

        markdownBuilder.AddTableRow("Application", permissionsStub, permissionsStub);
        markdownBuilder.EndTable();
        return markdownBuilder.ToString();
    }
}
