using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebPocHub.Dal
{
	public interface ICommonRepository<T>
	{
		Task<List<T>> GetAll();
		Task<T> GetDetails(int id);
		Task<T> Insert(T item);
		Task<T> Update(T item);
		Task<T> Delete(int id);
	}
}
