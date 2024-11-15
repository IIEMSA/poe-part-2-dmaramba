## Packages Required
- Install MySql.EntityFrameworkCore using Nuget Package Installer
- Install Microsoft.EntityFrameworkCore.InMemory using Nuget Package Installer

### Change the OnConfiguration settings of the context to be like this
```
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
     optionsBuilder.UseInMemoryDatabase(databaseName: "DatabaseDB");
 }

```

You can also use in sqlite datababse
