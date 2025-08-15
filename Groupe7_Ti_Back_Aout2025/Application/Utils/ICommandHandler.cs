namespace Application.Utils;

public interface ICommandHandler<I,O>
{
    O Handle(I command);
}