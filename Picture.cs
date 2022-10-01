namespace Game2048;

public class Picture
{
    private Gtk.Image image;

    public Picture(string resource) => image = Gtk.Image.LoadFromResource(resource);

    public Gtk.Image GetImage() => image;
    public Gtk.Image GetImage(int width, int height) =>
        new Gtk.Image(image.Pixbuf.ScaleSimple(width, height, Gdk.InterpType.Bilinear));
}

