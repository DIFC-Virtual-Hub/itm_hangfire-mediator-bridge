using Hangfire;
using System;
using MediatR;

namespace HangfireMediator
{
  public static class MediatorExtensions
  {
    /// <summary>
    /// Creates a background job based on a specified mediator request and places it into its fire-and-forget queue and are executed only once and almost immediately after creation.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <returns>Job Id</returns>
    public static string Enqueue(this IMediator mediator, IBaseRequest request)
    {
      var client = new BackgroundJobClient();
      return client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    /// <summary>
    /// Creates a background job based on a specified mediator request and places it into its fire-and-forget queue and are executed only once and almost immediately after creation.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="name">Name of the job to be displayed in Hangfire</param>
    /// <returns>Job Id</returns>
    public static string Enqueue(this IMediator mediator, IBaseRequest request, string name)
    {
      var client = new BackgroundJobClient();
      return client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(name, request));
    }

    /// <summary>
    /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="time"></param>
    /// <returns>Job Id</returns>
    public static string Delay(this IMediator mediator, IBaseRequest request, TimeSpan time)
    {
      var client = new BackgroundJobClient();
      return client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send( request), time );
    }

    /// <summary>
    /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="time"></param>
    /// <param name="name">Name of the job to be displayed in Hangfire</param>
    /// <returns>Job Id</returns>
    public static string Delay(this IMediator mediator, IBaseRequest request, TimeSpan time, string name)
    {
      var client = new BackgroundJobClient();
      return client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send( name, request), time );
    }

    /// <summary>
    /// Chains are executed when its parent job has been finished.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="jobId"></param>
    /// <returns>Child Job Id</returns>
    public static string Chain(this IMediator mediator, IBaseRequest request, string jobId)
    {
      var client = new BackgroundJobClient();
      return client.ContinueJobWith<MediatorHangfireBridge>(jobId, bridge => bridge.Send( request));
    }

    /// <summary>
    /// Chains are executed when its parent job has been finished.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="request"></param>
    /// <param name="jobId"></param>
    /// <param name="name">Name of the job to be displayed in Hangfire</param>
    /// <returns>Child Job Id</returns>
    public static string Chain(this IMediator mediator, IBaseRequest request, string jobId, string name)
    {
      var client = new BackgroundJobClient();
      return client.ContinueJobWith<MediatorHangfireBridge>(jobId, bridge => bridge.Send( name, request));
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
