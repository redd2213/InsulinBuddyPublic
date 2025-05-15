using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using Unity.Notifications.Android;
using System.Linq;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    public TMP_InputField hourInput;
    public TMP_InputField minuteInput;
    public TMP_Text nextAlarmCountdownText;
    public Button setAlarmButton;
    public GameObject alarmListPanel;
    public GameObject alarmBoxPrefab;

    private List<Alarm> alarms = new List<Alarm>();
    private const int maxAlarms = 6;
    public GameObject maxAlarmPopup;

    void Start()
    {
        // Register Android notification channel (required for notifications)
        RegisterNotificationChannel();

        setAlarmButton.onClick.AddListener(AddAlarm);
    }

    void Update()
    {
        UpdateCountdown();
    }

    void AddAlarm()
    {
        if (alarms.Count >= maxAlarms)
        {
            Debug.LogWarning("Maximum number of alarms reached!");
            maxAlarmPopup.SetActive(true);
            return;
        }

        if (int.TryParse(hourInput.text, out int hour) && int.TryParse(minuteInput.text, out int minute))
        {
            if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60)
            {
                Alarm newAlarm = new Alarm(hour, minute);
                alarms.Add(newAlarm);
                DisplayAlarm(newAlarm);
                ScheduleNotification(newAlarm); // Schedule the notification
                Debug.LogWarning("Alarm displayed!");
            }
            else
            {
                Debug.LogError("Invalid hour or minute input!");
            }
        }
        else
        {
            Debug.LogError("Hour and Minute must be valid integers!");
        }
    }

    void DisplayAlarm(Alarm alarm)
    {
        GameObject alarmBox = Instantiate(alarmBoxPrefab, alarmListPanel.transform);

        TMP_Text alarmText = alarmBox.transform.Find("AlarmText").GetComponent<TMP_Text>();
        alarmText.text = $"Alarm: {alarm.Hour:D2}:{alarm.Minute:D2}";

        Button deleteButton = alarmBox.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            alarms.Remove(alarm);
            Destroy(alarmBox);
        });
    }

    public void ClearAllAlarms()
    {
        // Clear the list of alarms
        alarms.Clear();

        // Destroy all the alarm UI elements in the alarmListPanel
        foreach (Transform child in alarmListPanel.transform)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("All alarms cleared!");
    }

    void UpdateCountdown()
    {
        if (alarms.Count == 0)
        {
            nextAlarmCountdownText.text = "No alarms set.";
            return;
        }

        DateTime now = DateTime.Now;
        TimeSpan? shortestTimeSpan = null;

        foreach (var alarm in alarms)
        {
            DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, alarm.Hour, alarm.Minute, 0);

            if (alarmTime <= now)
            {
                alarmTime = alarmTime.AddDays(1);
            }

            TimeSpan timeUntilAlarm = alarmTime - now;

            if (shortestTimeSpan == null || timeUntilAlarm < shortestTimeSpan)
            {
                shortestTimeSpan = timeUntilAlarm;
            }
        }

        if (shortestTimeSpan != null)
        {
            TimeSpan timeLeft = shortestTimeSpan.Value;
            nextAlarmCountdownText.text = $"Next dose in: {timeLeft.Hours:D2}Hrs {timeLeft.Minutes:D2}Mins";
        }
    }

    void ScheduleNotification(Alarm alarm)
    {
        DateTime now = DateTime.Now;
        DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, alarm.Hour, alarm.Minute, 0);

        if (alarmTime <= now)
        {
            alarmTime = alarmTime.AddDays(1);
        }

        double timeUntilAlarm = (alarmTime - now).TotalSeconds;

        // Create and schedule the notification
        var notification = new AndroidNotification
        {
            Title = "InsulinBuddy",
            Text = $"Your dose is scheduled for {alarm.Hour:D2}:{alarm.Minute:D2}!",
            SmallIcon = "default",
            FireTime = DateTime.Now.AddSeconds(timeUntilAlarm),
        };

        AndroidNotificationCenter.SendNotification(notification, "alarm_channel");
    }

    void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "alarm_channel",
            Name = "Alarm Notifications",
            Importance = Importance.Default,
            Description = "Notifications for alarm reminders",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
}

[Serializable]
public class Alarm
{
    public int Hour;
    public int Minute;

    public Alarm(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
    }
}
