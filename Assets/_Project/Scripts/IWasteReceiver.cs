public interface IWasteReceiver
{
    bool TryDeposit(WasteItem item);
}