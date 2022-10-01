using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2048;
public class GameOver
{
    public bool IsGameOver = false;
    public void GameOverWindow()
    {
        Window go = new Window(WindowType.Popup);
        Program.App.AddWindow(go);
        Program.Win.GetPosition(out int x, out int y);
        go.Move(x + (int)Math.Round(Program.Win.AllocatedWidth * 0.25), 
                y + (int)Math.Round(Program.Win.AllocatedHeight * 0.25));
        go.SetDefaultSize((int)Math.Round(Program.Win.AllocatedWidth * 0.5), 
                          (int)Math.Round(Program.Win.AllocatedHeight * 0.5));
        var label = new Label();
        label.Markup = "<span size='35000'>Game Over</span>";
        Button gameOverBtn = new Button(label);
        gameOverBtn.Clicked += (sender, e) =>
        {
            go.Close();
            go.Hide();
            go.Dispose();
            go.Destroy();
            Program.Restart();
        };

        go.Add(gameOverBtn);
        go.ShowAll();
    }

    public bool CheckGameOver(BlocksManager blocks)
    {
        if (blocks.BlocksList.Any(p => p.Value == 1))
            return false;

        for (int i = 0; i < GameParameters.RowColumnCount; i++)
        {
            if (blocks.BlocksMatrix.Row(i).Select(p => p.Value).HasDuplicates(1))
                return false;
            if (blocks.BlocksMatrix.Column(i).Select(p => p.Value).HasDuplicates(1))
                return false;
        }
        return true;
    }
}
