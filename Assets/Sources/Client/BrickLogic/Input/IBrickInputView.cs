using Server.BricksLogic;

namespace Client.Input
{
    internal interface IBrickInputView
    {
        IBrickInputPresenter Presenter { get; set; }
    }
}