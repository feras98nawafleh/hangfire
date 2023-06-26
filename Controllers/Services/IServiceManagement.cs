namespace FireApp.Services;
public interface IServiceManagement
{
    void SendEmail();
    void UpdateDatabase();
    void GenerateMerchandise();
    void SyncData();
}