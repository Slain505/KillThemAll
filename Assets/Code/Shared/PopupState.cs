using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Shared
{
	public class PopupState<T> : MonoBehaviour, IPopup
	{
		private CancellationTokenSource cancellationToken = null;

		public virtual void Init()
		{
		}

		public virtual void Dispose()
		{
		}

		public async UniTask<bool> Open()
		{
			OnBeforeTransitionOpen();
			var token = TaskUtils.RefreshToken(ref cancellationToken);
			await OpenTransition(token).SuppressCancellationThrow();

			OnOpen();

			await UniTask.WaitUntilCanceled(token);

			OnBeforeTransitionClose();
			await CloseTransition();
			OnClose();

			return true;
		}

		public void Close()
		{
			cancellationToken?.Cancel();
		}

		private async UniTask OpenTransition(CancellationToken ct)
		{
			var task2 = OnOpenTransition(ct);
			await UniTask.WhenAll(task2);
		}

		private async UniTask CloseTransition()
		{
			var task1 = OnCloseTransition();
			await UniTask.WhenAll(task1);
		}

		protected virtual void OnOpen()
		{
		}

		protected virtual void OnClose()
		{
		}

		protected virtual async UniTask OnOpenTransition(CancellationToken ct)
		{
			await UniTask.DelayFrame(1, cancellationToken: ct);
			gameObject.SetActive(true);
		}

		protected virtual async UniTask OnCloseTransition()
		{
			await UniTask.DelayFrame(1);
			gameObject.SetActive(false);
		}

		protected virtual void OnBeforeTransitionOpen()
		{
		}

		protected virtual void OnBeforeTransitionClose()
		{
		}
	}
}