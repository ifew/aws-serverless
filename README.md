# aws-serverless
Example .Net Core 2 with AWS Lambda

## Deploy with AWS Cli
### Profile (list profile)
`$ cd profile/src/profile`

`$ dotnet lambda deploy-function profile –-function-role MyRole`

1. enter role name `MyRole` (create Role in AWS Account)
2. enter handler `profile::profile.Function::Get`
3. enter memory `128`
4. enter timeout `3`
5. go to Lambda Function, and enter Environment variables such as
- key: TEST_LAMBDA_DBCONNECTION 
- value: server={HOST};userid={USERNAME};password={PASSWORD};database={DATABASE_NAME};convert zero datetime=True;
