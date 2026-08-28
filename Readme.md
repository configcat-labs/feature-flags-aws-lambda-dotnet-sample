# Feature Flags in Microservices and Serverless Architecture (AWS Lambda and .NET)

**[Read the blog post here](https://configcat.com/blog/feature-flags-aws-lambda-dotnet/)**

A sample app demonstrating how to use [ConfigCat feature flags](https://configcat.com) in an AWS Lambda function running on .NET.

## Build & Run

Instructions on how to build and run the application locally.

### Prerequisites

You will need a few tools installed and accounts set up. Free tiers work perfectly for everything here.

- **A ConfigCat account:** You can sign up for a [Forever Free account here](https://app.configcat.com/auth/signup).
- **An AWS account:** You will need access to the AWS Console and a user with permissions to create Lambda functions.
- **[Docker Desktop](https://www.docker.com/products/docker-desktop/):** You can use Docker to spin up a Redis server to run the Lambda locally. (In production AWS environments, Amazon ElastiCache is typically used instead.)
- **Microsoft .NET SDK:** You will need the .NET 10 SDK or newer to build the project. [Download it here](https://dotnet.microsoft.com/download).
- **Command line tools:**
  - **[AWS .NET Mock Lambda Test Tool](https://github.com/aws/aws-lambda-dotnet/blob/master/Tools/LambdaTestTool):** Install the version matching your .NET SDK version as a [global .NET tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools) (`dotnet tool install -g Amazon.Lambda.TestTool-10.0`).
  - **[AWS CLI](https://aws.amazon.com/cli/):** Configure it with your credentials (`aws configure`).
  - **[AWS Lambda Tools for .NET](https://github.com/aws/aws-extensions-for-dotnet-cli):** Install it as a [global .NET tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools) (`dotnet tool install -g Amazon.Lambda.Tools`).

### Local

1. Build the project:

   ```bash
   dotnet build
   ```

2. Spin up a Redis server locally using Docker:

   ```bash
   docker compose up -d
   ```

3. Run the Lambda Test Tool:

   ```bash
   dotnet lambda-test-tool-10.0
   ```

4. When launched, click the **Execute Function** button. The **Response** and **Log Output** show the result of feature flag evaluation, based on the data cached in Redis.

### AWS

1. Deploy the function to AWS Lambda:

   ``` bash
   dotnet lambda deploy-function ConfigCatLambdaDemo
   ```

2. Once deployed, invoke the function directly from the CLI to see the result immediately:

   ```bash
   dotnet lambda invoke-function ConfigCatLambdaDemo
   ```

## Learn more

Useful links to technical resources:

- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [AWS Lambda Documentation](https://docs.aws.amazon.com/lambda/)

[**ConfigCat**](https://configcat.com) supports many other frameworks and languages. Check out the full list of supported SDKs [here](https://configcat.com/docs/sdk-reference/overview/).

You can also explore other code samples for various languages, frameworks, and topics in [ConfigCat labs](https://github.com/configcat-labs) on GitHub.

Keep up with ConfigCat on [X](https://x.com/configcat), [Facebook](https://www.facebook.com/configcat), [LinkedIn](https://www.linkedin.com/company/configcat/), [GitHub](https://github.com/configcat), and the [News & Product Updates](https://configcat.com/docs/news/) page.

## Authors

- [adams85](https://github.com/adams85)
- [Chavez Harris](https://github.com/codedbychavez)

## Contributions

Contributions are welcome!
