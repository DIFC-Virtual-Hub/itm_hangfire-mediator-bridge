# IT Minds Hangfire Mediator Bridge

This library creates a simple bridge between MediatR and Hangfire, enabling hangfire to be used under the hood as a schedule/background job runner of the mediator pattern.

## Usage

The methods you will be using are entity extensions upon the `IMediator` interface.

### Setup: Hangfire Service Registration

Be sure to add the `UseMediatR` configuration extension for the Hangfire registration.

```csharp
services.AddHangfire(configuration =>
{
  // ...
  configuration.UseMediatR();
});
```

### Enqueueing jobs

Following the setup, you can enqueue your jobs as part of the mediator pattern to run in the background. All you have to do is instead of `.Send` you write `.Enqueue`. Note: current implementation is limited to Enqueue being a void method.

```csharp
mediator.Enqueue(new ExampleQuery { });
```

### Optional: Setup recurring jobs

You can easily register recurring jobs with the `.Enqueue` method.

Here is an example of a mass registration:

```csharp
using HangfireMediator;

public static IMediator SetupHangfireJobs(this IMediator mediator)
{
  mediator.RecurringJob(
    new ExampleQuery { },
    "Hourly Example Query",
    Cron.Hourly()
  );

  //...

  return mediator;
}
```
