namespace Code.Shared
{
	public class PopupStateModel<T, V> : PopupState<T>, IPopupModel<V> where V : IModel
	{
		protected V model;
		
		public virtual void Setup(V m)
		{
			model = m;
		}
	}
}