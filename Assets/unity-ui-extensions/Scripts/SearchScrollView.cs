using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	public class SearchScrollView : FancyScrollView<Track, SearchScrollViewContext>
	{
		[SerializeField]
		ScrollPositionController scrollPositionController;

		new void Awake()
		{
			scrollPositionController.OnUpdatePosition.AddListener(UpdatePosition);
			// Add OnItemSelected event listener
			scrollPositionController.OnItemSelected.AddListener(CellSelected);

			SetContext(new SearchScrollViewContext { OnPressedCell = OnPressedCell });
			base.Awake();
		}

		public void UpdateData(List<Track> data)
		{
			cellData = data;
			scrollPositionController.SetDataCount(cellData.Count);
			UpdateContents();
		}

		void OnPressedCell(SearchScrollViewCell cell)
		{
			scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			context.SelectedIndex = cell.DataIndex;
			UpdateContents();
		}

		// An event triggered when a cell is selected.
		void CellSelected(int cellIndex)
		{
			// Update context.SelectedIndex and call UpdateContents for updating cell's content.
			context.SelectedIndex = cellIndex;
			UpdateContents();
		}
	}
}
