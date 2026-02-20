using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public RectTransform parent;
    public Card cellPrefab;
    public Card[] cells;

      int rows = 3;
      int columns = 4;

    public float cellAspectRatio = 1f;

    public Vector2 spacing;

    public Card[] GenerateGrid(RectTransform parent, GameObject cellPrefab, int rows = 3, int columns = 4)
    {
        this.parent = parent;
        this.cellPrefab = cellPrefab.GetComponent<Card>();
        this.rows = rows;
        this.columns = columns;
        GenerateGrid();
        return cells;
    }
      
    public void GenerateGrid()
    {

        // Clear old children
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            if (Application.isEditor)
                DestroyImmediate(parent.GetChild(i).gameObject);
            else
                Destroy(parent.GetChild(i).gameObject);
        }

        float parentWidth = parent.rect.width;
        float parentHeight = parent.rect.height;

        float totalSpacingX = (columns - 1) * spacing.x;
        float totalSpacingY = (rows - 1) * spacing.y;

        float availableWidth = parentWidth - totalSpacingX;
        float availableHeight = parentHeight - totalSpacingY;

        // Calculate cell size
        float cellWidth = availableWidth / columns;
        float cellHeight = cellWidth / cellAspectRatio;

        if ((cellHeight * rows) > availableHeight)
        {
            cellHeight = availableHeight / rows;
            cellWidth = cellHeight * cellAspectRatio;
        }

        //   Total grid size
        float totalGridWidth = (cellWidth * columns) + totalSpacingX;
        float totalGridHeight = (cellHeight * rows) + totalSpacingY;

        //   Start from center
        float startX = -totalGridWidth / 2 + cellWidth / 2;
        float startY = totalGridHeight / 2 - cellHeight / 2;
        startY = parentHeight / 2 - cellHeight / 2;


        cells = new Card[rows * columns];


        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Card cell = Instantiate(cellPrefab, parent);

                RectTransform cellRTR = cell.transform.GetComponent<RectTransform>();

                cellRTR.localScale = Vector3.one;
                cellRTR.sizeDelta = new Vector2(cellWidth, cellHeight);

                // IMPORTANT: Ensure center anchor
                cellRTR.anchorMin = cellRTR.anchorMax = new Vector2(0.5f, 0.5f);
                cellRTR.pivot = new Vector2(0.5f, 0.5f);

                float x = startX + c * (cellWidth + spacing.x);
                float y = startY - r * (cellHeight + spacing.y);

                cellRTR.anchoredPosition = new Vector2(x, y);


                int index = r * columns + c;
                cells[index] = cell;
                cells[index].CardId = index;
            }
        }
    }
}