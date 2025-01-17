namespace Basket.API.Exceptionos;

public class BasketNotFoundException : NotFoundException
{
	public BasketNotFoundException(string userName) : base("basket", userName)
	{
	}
}