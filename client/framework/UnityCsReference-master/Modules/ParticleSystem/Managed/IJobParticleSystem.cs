// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License


using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs
{
    [JobProducerType(typeof(ParticleSystemJobStruct<>))]
    public interface IJobParticleSystem
    {
        void Execute(ParticleSystemJobData jobData);
    }

    [JobProducerType(typeof(ParticleSystemParallelForJobStruct<>))]
    public interface IJobParticleSystemParallelFor
    {
        void Execute(ParticleSystemJobData jobData, int index);
    }

    [JobProducerType(typeof(ParticleSystemParallelForBatchJobStruct<>))]
    public interface IJobParticleSystemParallelForBatch
    {
        void Execute(ParticleSystemJobData jobData, int startIndex, int count);
    }

    public static class IParticleSystemJobExtensions
    {
        unsafe public static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, JobHandle dependsOn = new JobHandle()) where T : struct, IJobParticleSystem
        {
            var scheduleParams = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemJobStruct<T>.Initialize());
            var handle = ParticleSystem.ScheduleManagedJob(ref scheduleParams, ps.GetManagedJobData());
            ps.SetManagedJobHandle(handle);
            return handle;
        }

        unsafe public static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, int minIndicesPerJobCount, JobHandle dependsOn = new JobHandle()) where T : struct, IJobParticleSystemParallelFor
        {
            var scheduleParams = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemParallelForJobStruct<T>.Initialize());
            var handle = JobsUtility.ScheduleParallelForDeferArraySize(ref scheduleParams, minIndicesPerJobCount, ps.GetManagedJobData(), null);
            ps.SetManagedJobHandle(handle);
            return handle;
        }

        unsafe public static JobHandle ScheduleBatch<T>(this T jobData, ParticleSystem ps, int innerLoopBatchCount, JobHandle dependsOn = new JobHandle()) where T : struct, IJobParticleSystemParallelForBatch
        {
            var scheduleParams = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemParallelForBatchJobStruct<T>.Initialize());
            var handle = JobsUtility.ScheduleParallelForDeferArraySize(ref scheduleParams, innerLoopBatchCount, ps.GetManagedJobData(), null);
            ps.SetManagedJobHandle(handle);
            return handle;
        }

        unsafe private static JobsUtility.JobScheduleParameters CreateScheduleParams<T>(ref T jobData, ParticleSystem ps, JobHandle dependsOn, IntPtr jobReflectionData) where T : struct
        {
            dependsOn = JobHandle.CombineDependencies(ps.GetManagedJobHandle(), dependsOn);
            return new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), jobReflectionData, dependsOn, ScheduleMode.Batched);
        }
    }
}

