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

## Implantando no Azure

### 1. Usando o Kudu Debug console

Crie o arquivo **applicationHost.xdt** na raiz do site. Ex: **D:\home\site\applicationHost.xdt**:

```
cd D:\home\site
@echo off > applicationHost.xdt
```

Edite o conteúdo do mesmo para:

```xml
<?xml version="1.0"?> 
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform"> 
  <system.webServer> 
    <runtime xdt:Transform="InsertIfMissing">
      <environmentVariables xdt:Transform="InsertIfMissing">
        <add name="Auth:ConnectionString" value="filename=webapp-auth.db" xdt:Locator="Match(name)" xdt:Transform="InsertIfMissing" />
        <add name="Auth:DefaultRootUser" value="[rootUserName]" xdt:Locator="Match(name)" xdt:Transform="InsertIfMissing" />
        <add name="Auth:DefaultRootPassword" value="[rootPassword]" xdt:Locator="Match(name)" xdt:Transform="InsertIfMissing" />
        <add name="Hosting:Environment" value="[Development|Staging|Production]" xdt:Locator="Match(name)" xdt:Transform="InsertIfMissing" />
      </environmentVariables>
    </runtime> 
  </system.webServer> 
</configuration> 
```

### 2. Reinicie o site no Azure