using System;

namespace KokaarQrCoder.Infrastructure.Contracts
{
    public interface ILoggerService 
    { 
        void LogInformation(string message); 
        void LogWarning(string message); 
        void LogDebug(string message);
        void LogError(string message);
        void LogError(Exception ex);
    }
}
