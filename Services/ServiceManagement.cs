namespace FireApp.Services;
public class ServiceManagement : IServiceManagement
{
    void IServiceManagement.GenerateMerchandise()
    {
        Console.WriteLine($"Generating Merchandise... Long running task {DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm")}");
    }

    void IServiceManagement.SendEmail()
    {
        Console.WriteLine($"Sending Email... Short running task {DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm")}");
    }

    void IServiceManagement.SyncData()
    {
        Console.WriteLine($"Syncing Data... Short running task {DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm")}");
    }

    void IServiceManagement.UpdateDatabase()
    {
        Console.WriteLine($"Updating Database... Long running task {DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm")}");
    }
}