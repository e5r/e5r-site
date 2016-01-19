E5R Development Team Site
=========================

A web site for E5R Development Team. _(Under construction!)_

Production: **http://e5rdev.com**

Staging: **http://www-stating.e5rdev.com**

## Preparando o ambiente de desenvolvimento

### 1. Instale o ambiente ASP.NET 5.

Veja as instruções em https://docs.asp.net/en/latest/getting-started/index.html

### 2. Crie o arquivo de configurações do ambiente de desenvolvimento para o projeto web.

Faça uma cópia de `webapp.json` para `webapp.development.json`.

```shellscript
# Windows
copy "src\E5R.Product.WebSite\webapp.json" "src\E5R.Product.WebSite\webapp.development.json"

# Unix
cp 'src/E5R.Product.WebSite/webapp.json' 'src/E5R.Product.WebSite/webapp.development.json'
```

Preencha os dados no novo arquivo que acaba de criar.

### 3. Execute o Watch de desenvolvimento

```
run-dev
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
        <add name="Hosting:Environment" value="[Development|Staging|Production]" xdt:Locator="Match(name)" xdt:Transform="InsertIfMissing" />
      </environmentVariables>
    </runtime> 
  </system.webServer> 
</configuration> 
```

### 2. Crie o arquivo de configurações de ambiente `webapp.{env_name}.json`

Faça uma cópia de `webapp.json` para `webapp.{env_name}.json`.

```shellscript
# Windows
copy "src\E5R.Product.WebSite\webapp.json" "src\E5R.Product.WebSite\webapp.{env_name}.json"

# Unix
cp 'src/E5R.Product.WebSite/webapp.json' 'src/E5R.Product.WebSite/webapp.{env_name}.json'
```

Preencha os dados no novo arquivo que acaba de criar, e faça o upload do mesmo na pasta de aplicação
no Azure.

Ex: **D:\home\site\approot\packages\E5R.Product.WebSite\0.1.0-alpha4\root\webapp.{env_name}.json**:


### 3. Reinicie o site no Azure
