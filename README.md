# Create a new Signing Key For Identity Server Token Signing

Certificate tools can be found in the Windows SKD directory:

```C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x86```

### Generate a new Private Key and Certificate:

```makecert -n "CN=IdentityServer" -a sha256 -sv IdentityServer.pvk -r IdentityServer.cer```

### Export the Private/Public key as a Personal Information Exchange file:

```pvk2pfx -pvk IdentityServer.pvk -spc IdentityServer.cer```

When prompted:
* Enter password
* Select 'Yes, export the private key'
* Select 'Personal Information Exchange' option and check:
    * Include all certificates in the certification path if possible
    * Export all extended properties
    * Enable certificate privacy
* Set a password using 'Aes256-Sha256`


### Database migrations

#### Initialize AspNetIdentity
```dotnet ef migrations add InitialUserIdentityConfigurationDbMigration -c TenantUserDbContext -o Persistence/Migrations/IdentityServer/AspNetIdentity```

#### Initialize Configuration Tables
```dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Persistence/Migrations/IdentityServer/ConfigurationDb```

#### Initialize Persisted Grant Tables
```dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Persistence/Migrations/IdentityServer/PersistedGrantDb```

#### Migrated in code

```
public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
{
    if (environment.IsDevelopment())
    {
        InitializeDatabase(app);
        ...
    }
    ...
}

private void InitializeDatabase(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
        serviceScope.ServiceProvider.GetRequiredService<TenantUserDbContext>().Database.Migrate();
    }
}
```