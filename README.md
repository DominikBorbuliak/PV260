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

`dotnet ef migrations add <MigrationName> -s .\PV260.Project.Server\ -p .\PV260.Project.Infrastructure\ -o .\Persistence\Migrations\`

Update the database:

`dotnet ef database update -s .\PV260.Project.Server\ -p .\PV260.Project.Infrastructure\`

## SMTP Configuration

To send emails, you will have to configure smtp credentials.

### Ethereal

The easiest way is to use [Ethereal](https://ethereal.email/create). It provides smtp server for development purposes. Copy the credentials into configuration under `SMTP` key:

```json
...
"SMTP": {
    "Email": "example@ethereal.email",
    "Password": "ExampleEtherealPassword1.",
}
...
```

You shouldn't add these into `appsettings.json`, instead use _.NET User Secrets_ as described below.

#### Creating .NET User Secrets

**Rider** \
Right click on `V2P60.Project.Server` project -> Tools -> .NET User Secrets

**VisualStudio** \
Right click on `V2P60.Project.Server` project -> Manage User Secrets

**CLI**

```
$ dotnet user-secrets set "SMTP:Email" "email@gmail.com"
$ dotnet user-secrets set "SMTP:Password" "ExampleEmailPassword"
```

You can then open the [Ethereal Inbox](https://ethereal.email/messages) to check emails. **Emails won't be sent to the recipient!**

### Other SMTP servers

In order to use any other SMTP server, such as Gmail, you need to update `Host` in the `appsettings.json`:

```json
"SMTP": {
    "Host": "smtp.gmail.com",
    ...
}
```

Then you can configure credentials same as in the previous section.
