# PV260 Project

---

## Generated API client in React

### How to update generated client?

1. Start the server
2. Download new open api definition by running `npm run fetch:apidef` on client
3. Generate new api client by running `generate:client` on client

### How to use generated client?

- You can access api client via ApiClient instance
- Instance is exported in `~/src/services/api/base.ts` as `apiClient`

## Migrations

Run these from the _PV260.Project/_ directory.

Add new migration:

`dotnet ef migrations add <MigrationName> -s .\PV260.Project.Server\ -p .\PV260.Project.DataAccessLayer\`

Update the database:

`dotnet ef database update -s .\PV260.Project.Server\ -p .\PV260.Project.DataAccessLayer\`
