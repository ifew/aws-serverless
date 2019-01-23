# aws-serverless
Example .Net Core 2 with AWS Lambda

## Deploy with AWS Cli
### Profile (list profile)
**Run Unit Test**
```
$ cd profile/test/profile.Test
$ dotnet test
```

**Try to Run and Get result**
```
$ cd profile/src/profile
$ aws lambda invoke --function-name {LAMBDA_FUNCTION_NAME} output.txt
```

**Deploy**
```
$ dotnet lambda deploy-function {LAMBDA_FUNCTION_NAME} –-function-role {ROLE_NAME}
```


**IF DEPLOY AT FIRST TIME, AND CREATE NEW LAMBDA FUCNTION ON AWS**
1. enter role name `MyRole` (create Role in AWS Account)
2. enter handler `profile::profile.Function::Get`
3. enter memory `128`
4. enter timeout `3`
5. go to Lambda Function, and enter Environment variables such as
- key: TEST_LAMBDA_DBCONNECTION 
- value: server={HOST};userid={USERNAME};password={PASSWORD};database={DATABASE_NAME};convert zero datetime=True;
6. Config Security group, vpn, subnet on Labmda Function for access on RDS
