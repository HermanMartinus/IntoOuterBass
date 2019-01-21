namespace UnityEngine.UI.Extensions.Examples
{
    public class SearchScrollViewCell
        : FancyScrollViewCell<Track, SearchScrollViewContext>
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        GameObject cassette;
        [SerializeField]
        Button button;



        readonly int scrollTriggerHash = Animator.StringToHash("scroll");
        SearchScrollViewContext context;

        void Start()
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;
            UpdatePosition(0);

            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// コンテキストを設定します
        /// </summary>
        /// <param name="context"></param>
        public override void SetContext(SearchScrollViewContext context)
        {
            this.context = context;
        }

        public override void UpdateContent(Track track)
        {
            cassette.GetComponentInChildren<Text>().text = track.title;
            if (cassette.transform.Find("Backer").GetComponent<Image>().color == Color.white)
            {
                cassette.transform.Find("Backer").GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }

            if (context != null)
            {
                var isSelected = context.SelectedIndex == DataIndex;
                if(isSelected)
                {
                    SearchScreen.selectedSong = track;
                }
                //image.color = isSelected
                //? new Color32(225, 115, 13, 255)
                //: new Color32(255, 255, 255, 255);
            }
        }

        /// <summary>
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
        public override void UpdatePosition(float position)
        {
            animator.Play(scrollTriggerHash, -1, position);
            animator.speed = 0;
        }

        public void OnPressedCell()
        {
            if (context != null)
            {
                context.OnPressedCell(this);
            }
        }
    }
}
