using System;

public class Timer
{
    DateTime start;
    //Прошедшее время
    float pastTime = -1;
    //Продолжительность
    TimeSpan duration;

    public void Start(float _pastTime)
    {
        start = DateTime.Now;
        pastTime = _pastTime;
        duration = TimeSpan.Zero;
    }

    public void Update()
    {
        if (pastTime > 0)
        {
            duration = DateTime.Now - start;
            if (duration.TotalSeconds > pastTime)
            {
                pastTime = 0;
            }
        }
    }

    public bool CanFire()
    {
        return pastTime == 0;
    }
}
