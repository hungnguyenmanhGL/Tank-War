using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVar
{
    public enum skill {EMP_BOMB, BARRAGE, MISSILE_LOCK, SHIELD, SHOOT_LASER, PLASMA_CANNON};

    public static string obsBlockShellTag = "Obstacle";
    public static string obsBlockTankTag = "Low Obstacle";

    public static int eTankLayer = 7;
    public static int eIgnoreLowBlockLayer = 12;

    public static int aTankLayer = 6;
    public static int aIgnoreLowBlockLayer = 13;

    public static string shellTag = "Shell";
    public static string missileTag = "Missile";

    public static string pTag = "Player";
    public static string allyTag = "Ally";
    public static string eTag = "Enemy";
    public static string hpBar = "HP Bar";

    public static string firingFlash = "Firing Flash";
    public static string explosion = "Explosion";
    public static string tankExplosion = "Tank Explosion";

    public static string missionFailQuote = "Mission failed! We'll get them next time.";

    public static void MoveAndRotateTowardDes(Transform des, Transform obj, float speed)
    {
        Vector2 dirToDes = (des.position - obj.position).normalized;
        Vector2 goingToPos = obj.position;
        goingToPos.x += Time.deltaTime * speed * dirToDes.x;
        goingToPos.y += Time.deltaTime * speed * dirToDes.y;
        obj.position = goingToPos;

        float angle = Mathf.Atan2(dirToDes.y, dirToDes.x) * Mathf.Rad2Deg - 90;
        obj.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    } 

    public static GameObject GetEffect(string effectName, Transform i)
    {
        if (effectName == firingFlash)
        {
            return GetFiringFlash(i);
        }
        if (effectName == explosion)
        {
            return GetExplosion(i);
        }
        return null;
    }

    public static GameObject GetFiringFlash(Transform i)
    {
        GameObject flash = ObjectPool.instance.GetObjectForType(firingFlash, false);
        flash.transform.position = i.transform.position;
        flash.transform.rotation = i.transform.rotation;
        flash.transform.parent = i;
        return flash;
    }

    public static GameObject GetExplosion(Transform i)
    {
        GameObject flash = ObjectPool.instance.GetObjectForType(explosion, false);
        flash.transform.position = i.transform.position;
        flash.transform.rotation = i.transform.rotation;
        return flash;
    }

    public static GameObject GetObject(string objName, Transform i)
    {
        GameObject obj = ObjectPool.instance.GetObjectForType(objName, false);
        obj.transform.position = i.transform.position;
        obj.transform.rotation = i.transform.rotation;
        return obj;
    }

    public static GameObject FireShell(string shellName, Transform firingPoint, float force)
    {
        GetFiringFlash(firingPoint);

        GameObject shell = ObjectPool.instance.GetObjectForType(shellName, false);
        shell.transform.position = firingPoint.transform.position;
        shell.transform.rotation = firingPoint.transform.rotation;

        Bullet shellComp = shell.GetComponent<Bullet>();
        shellComp.SetStartingPoint(firingPoint);

        Rigidbody2D body = shell.GetComponent<Rigidbody2D>();
        body.AddForce(firingPoint.up * force, ForceMode2D.Impulse);

        return shell;
    }

    public static string convertTime(float timeAmount)
    {
        string result = " ";
        float timeInSecond = timeAmount;
        if (timeInSecond > 3600)
        {
            int timeInHour = (int)timeInSecond / 3600;
            if (timeInHour < 10) result += "0" + timeInHour + ":";
            else result += timeInHour + ":";
            timeInSecond -= timeInHour * 3600;
        }
        else result += "0:";

        if (timeInSecond > 60)
        {
            int timeInMinute = (int)timeInSecond / 60;
            if (timeInMinute < 10) result += "0" + timeInMinute + ":";
            else result += timeInMinute + ":";
            timeInSecond -= timeInMinute * 60;
        }
        else result += "0:";

        if (timeInSecond < 10) result += "0" + (int)timeInSecond;
        else result += (int)timeInSecond;
        return result;
    }

    public static float GetAngleBetweenPoints(Vector3 a, Vector3 b)
    {
        Vector3 dir = (a - b).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        return angle;
    }

    public static Dictionary<skill, string> skillNameMap = new Dictionary<skill, string>
    {
        {skill.EMP_BOMB, "EMP Bomb"},
        {skill.BARRAGE, "Barrage" },
        {skill.MISSILE_LOCK, "A-Missiles" },
        {skill.SHIELD, "Shield" },
        {skill.PLASMA_CANNON, "Plasma Cannon" }
    };
    public static Dictionary<int, Vector3> playerSpawnMap = new Dictionary<int, Vector3>
    {
        {4, new Vector3(-87,8,0)}
    };
}