namespace Application.Utils;

public interface IQueryHandler<I,O>
{
    O Handle(I query);
}