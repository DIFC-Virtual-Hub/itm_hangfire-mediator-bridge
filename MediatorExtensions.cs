using Hangfire;
using System;
using MediatR;

namespace HangfireMediator
{
  public static class MediatorExtensions
  {
    /// <summary>
    /// Creates a background job based on a specified mediator request and places it into its actual queue.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    public static void Enqueue(this IMediator mediator, IBaseRequest request)
    {
      var client = new BackgroundJobClient();
      client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    /// <summary>
    /// Creates a background job based on a specified mediator request and places it into its actual queue.
    /// Includes an easily readable given name for the hangfire dashboard and log.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    public static void Enqueue(this IMediator mediator, IBaseRequest request, string name)
    {
      var client = new BackgroundJobClient();
      client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(name, request));
    }

    public static void RecurringJob(this IMediator mediator, IBaseRequest request, string name, Func<string> cronExpression)
    {
      var client = new RecurringJobManager();
      client.AddOrUpdate<MediatorHangfireBridge>(name, bridge => bridge.Send(name, request), cronExpression);
    }

    public static void RecurringJob(this IMediator mediator, IBaseRequest request, string name, string cronExpression)
    {
      var client = new RecurringJobManager();
      client.AddOrUpdate<MediatorHangfireBridge>(name, bridge => bridge.Send(name, request), cronExpression);
    }
  }
}
