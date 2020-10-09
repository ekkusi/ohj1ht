using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class LapsyLintu : PhysicsGame
{
    private IntMeter pisteLaskuri;
    private PhysicsObject lintu;
    public override void Begin()
    {
        AlustaPeli();
        SeinienLuontiAjastus();
        AlustaPisteLaskuri();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    public void AlustaPeli()
    {
        lintu = new PhysicsObject(50, 50, Shape.Rectangle);
        lintu.Color = Color.Black;
        lintu.X = Level.Left + lintu.Width * 2;
        Add(lintu);

        AddCollisionHandler(lintu, LintuTormays);

        Keyboard.Listen(Key.Space, ButtonState.Released, HyppaaLinnulla, "Lintu hyppää ylöspäin");

        Camera.ZoomToLevel();
        Level.CreateBorders();

        Gravity = new Vector(0.0, -400);
    }
    public void HyppaaLinnulla()
    {
        lintu.Hit(new Vector(0.0, 200));
    }

    public void SeinienLuontiAjastus()
    {
        Timer ajastin = new Timer();
        ajastin.Interval = 2;
        ajastin.Timeout += LisaaSeinat;
        ajastin.Start();
    }

    public void AlustaPisteLaskuri()
    {
        pisteLaskuri = new IntMeter(0);

        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Left + 100;
        pisteNaytto.Y = Screen.Top - 100;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;

        pisteNaytto.BindTo(pisteLaskuri);
        Add(pisteNaytto);
    }

    public void LisaaSeinat()
    {
        double valinKorkeus = 100;
        double alaSeinaKorkeus = RandomGen.NextDouble(0, Level.Height - valinKorkeus);
        double ylaSeinaKorkeus = Level.Height - alaSeinaKorkeus - valinKorkeus;

        PhysicsObject alaSeina = PhysicsObject.CreateStaticObject(20, alaSeinaKorkeus);
        alaSeina.X = Level.Right;
        alaSeina.Y = Level.Bottom + alaSeinaKorkeus / 2;
        alaSeina.Color = Color.Black;
        alaSeina.Tag = "alapalkki";
        Add(alaSeina);

        PhysicsObject ylaSeina = PhysicsObject.CreateStaticObject(20, ylaSeinaKorkeus);
        ylaSeina.X = Level.Right;
        ylaSeina.Y = Level.Bottom + alaSeinaKorkeus + valinKorkeus + ylaSeinaKorkeus / 2;
        alaSeina.Color = Color.Black;
        ylaSeina.Tag = "ylapalkki";
        Add(ylaSeina);

        alaSeina.Velocity = new Vector(-100, 0);
        ylaSeina.Velocity = new Vector(-100, 0);
    }

    public void LintuTormays(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        MessageDisplay.Add("Bump to: " + kohde.Tag);
        AloitaAlusta();
    }

    public void AloitaAlusta()
    {
        ClearAll();

        AlustaPeli();
        pisteLaskuri.SetValue(0);
    }
}

