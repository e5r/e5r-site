E5R Development Team Site
=========================

A web site for E5R Development Team. _(Under construction!)_

## Preparando o ambiente de desenvolvimento

### 1. Instale o ambiente ASP.NET 5.

Veja as instruções em https://docs.asp.net/en/latest/getting-started/index.html

### 2. Instale as ferramentas **SecretManager** e **dnx-watch**

```
dnu commands install Microsoft.Framework.SecretManager
dnu commands install Microsoft.Dnx.Watcher
```

### 3. Configure os dados sensíveis (usuários, senhas, etc) de usuário no projeto

* __Auth:ConnectionString__: Connection string para banco SQLite de autenticação/autorização
* __Auth:DefaultRootUser__: Nome do usuário ROOT padrão
* __Auth:DefaultRootPassword__: Senha do usuário ROOT padrão

```
user-secret set Auth:ConnectionString "filename=webapp-auth.db" --project ./src/E5R.Product.WebSite
user-secret set Auth:DefaultRootUser "[rootUserName]" --project ./src/E5R.Product.WebSite
user-secret set Auth:DefaultRootPassword "[rootPassword]" --project ./src/E5R.Product.WebSite
```

### 4. Execute a aplicação _zumbi_ para iniciar o desenvolvimento

```
watch
```
