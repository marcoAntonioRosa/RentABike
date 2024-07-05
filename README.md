# Rent A Bike 🏍️💨 Aluguel de motos

## AWS CLI
Para que o S3 funcione você vai precisar ter uma conta no AWS. Pode ser uma conta no plano gratuito.

Você precisa configurar um usuário no Identity and Access Management (IAM), certifique-se de dar permissão de administrador (AdministratorAccess).

Com o usuário criado, gere uma chave de acesso (Access Key e Secret Key) e faça download dessas informações.

Instale o AWS CLI (aws.amazon.com/cli) e configure com as chaves de acesso que você gerou.

Você também vai precisar criar um bucket no S3, guarde o nome do bucket.

## Configurando o AppSettings
Para que a web api funcione é necessário configurar os serviços de infraestrutura no AppSettings

### Conexão com o S3
```json
"AWS": {
    "BucketName": "{NOME_DO_BUCKET}" 
},
```

Importante, o S3 não funcionará se o IAM não estiver configurado.

### Conexão com o PostgreSQL

```json
"ConnectionStrings": {
    "PostgreSql": "Server={IP_DO_SERVER};Port={PORTA};Database={NOME_DO_BANCO_DE_DADOS};User Id={NOME_DO_USUARIO};Password={SENHA_DO_BANCO};"
},
```

### Configurando o JWT
```json
"JWTSettings": {
    "SecurityKey": "{DEFINA_SUA_SECURITY_KEY_AQUI}",
    "ValidIssuer": "RentABike",
    "ValidAudience": "{VALID_AUDIENCE}",
    "ExpiryInMinutes": {TEMPO_EM_MINUTOS}
},
```

## Construindo o banco de dados
Agora você precisa criar as migrações pelo Entity Framework. 
É mais fácil pelo JetBrains Rider, mas da para fazer pela linha de comando também.

```
ef migrations add --project RentABike.Infrastructure/RentABike.Infrastructure.csproj --startup-project RentABike/RentABike.csproj --context RentABike.Infrastructure.Persistence.PostgreSqlDbContext --configuration Debug Initial --output-dir Migrations
```
Agora você precisa realizar as alterações no banco.

```
ef database update --project RentABike.Infrastructure/RentABike.Infrastructure.csproj --startup-project RentABike/RentABike.csproj --context RentABike.Infrastructure.Persistence.PostgreSqlDbContext --configuration Debug 20240705055153_Initial
```
Note que no final da linha você deve alterar o nome do arquivo de Migrations para o nome do arquivo que foi gerado pra você.

## Fazendo requisições
Adicionei um arquivo chamado "requests.JSON". Ele pode ser importado no Insomnia para fazer as requisições para a API.

## Criando usuários e admins
Na aplicação, administradores são chamados de 'Admins' e usuários de 'DeliveryPerson'.
Ao criar um usuário, você precisa atribuir sua role manualmente. 
Em `RentABike.Controllers.AccountController` no método `RegisterUser` alterne entre
`await userManager.AddToRoleAsync(user, "Admin");` e `await userManager.AddToRoleAsync(user, "DeliveryPerson");` para criar os usuários conforme necessário.

Estou sempre aberto a feedbacks!
Qualquer dúvida me chama =D
