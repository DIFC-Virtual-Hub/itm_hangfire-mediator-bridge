# IT Minds Hangfire Mediator Bridge

This library creates a simple bridge between MediatR and Hangfire enabling hangfire to be used under the hood as a schedule/background job runner of the mediator pattern.

## Usage

### Hangfire Service Registration

Be sure to add the `UseMediatR` configuration extension for the Hangfire registration.

```csharp
services.AddHangfire(configuration =>
{
  configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);

  // ...
  configuration.UseMediatR();
});
```

### Enqueueing jobs

Following the setup you can enqueue as part of the mediator pattern to run jobs in the background as part of the Hangfire queue system. All you have to do is instead of `.Send` you write `.Enqueue`.

```csharp
mediator.Enqueue(new ExampleQuery { });
```

### Optional: Setup recurring jobs

You can easily register recurring jobs like this:

```csharp
using HangfireMediator;

public static IMediator SetupHangfireJobs(this IMediator mediator)
{
  mediator.RecurringJob(
    new ExampleQuery { },
    "Hourly Example Query",
    Cron.Hourly()
  );

  return mediator;
}
```
