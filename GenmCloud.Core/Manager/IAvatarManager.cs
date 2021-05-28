namespace GenmCloud.Core.Manager
{
    interface IAvatarManager
    {
        void Store(uint id, string avatar);

        string Get(uint id);
    }
}
