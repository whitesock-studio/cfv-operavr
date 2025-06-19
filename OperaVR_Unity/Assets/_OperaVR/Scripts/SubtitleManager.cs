namespace OperaVR
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Text.RegularExpressions;
	using UnityEngine;
	using UnityEngine.Events;
	using static UnityEngine.EventSystems.EventTrigger;

	[System.Serializable]
	public class SubtitleEntry
	{
		public float startTime;
		public float endTime;
		public string text;
	}

	public class SubtitleEvent : UnityEvent<string> { }

	public class SubtitleManager : MonoBehaviour
	{
		[SerializeField] AudioSource audioSource;
		[SerializeField] TextAsset subtitleFile; // Inserisci qui il file .srt come TextAsset
		public SubtitleEvent OnSubtitleChanged = new SubtitleEvent();

		[SerializeField] List<SubtitleEntry> subtitles = new List<SubtitleEntry>();
		private int currentIndex = 0;
		private string lastSubtitle = "";

		public void Awake()
		{
			OnSubtitleChanged.AddListener(DebugEntry);
			if (audioSource == null || subtitleFile == null)
			{
				Debug.LogError("SubtitleManager: AudioSource o SubtitleFile mancante.");
				return;
			}

			ParseSRT(subtitleFile.text);
		}

		public void Play()
		{
			audioSource.Play();
			StartCoroutine(SubtitleCoroutine());
		}

		public void Stop()
		{
			audioSource.Stop();
		}

		private void DebugEntry(string entry)
		{
			Debug.Log($"[SUBTITLE] {entry}");
		}

		private IEnumerator SubtitleCoroutine()
		{
			while (audioSource.isPlaying)
			{
				float currentTime = audioSource.time;
				if (currentIndex < subtitles.Count)
				{
					SubtitleEntry current = subtitles[currentIndex];
					if (currentTime >= current.startTime && currentTime <= current.endTime)
					{
						if (current.text != lastSubtitle)
						{
							lastSubtitle = current.text;
							OnSubtitleChanged.Invoke(current.text);
						}
					}
					else if (currentTime > current.endTime)
					{
						currentIndex++;
						lastSubtitle = "";
						OnSubtitleChanged.Invoke(""); // Pulisce i sottotitoli
					}
				}

				yield return null;
			}

			// Fine audio: svuota sottotitoli
			OnSubtitleChanged.Invoke("");
		}

		private void ParseSRT(string srt)
		{
			subtitles.Clear();
			string[] entries = Regex.Split(srt.Trim(), @"\r\n\r\n|\n\n");

			foreach (string entry in entries)
			{
				string[] lines = entry.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
				if (lines.Length >= 3)
				{
					string timeLine = lines[1];
					string[] timeParts = timeLine.Split(new[] { " --> " }, StringSplitOptions.None);

					float start = ParseTimecode(timeParts[0]);
					float end = ParseTimecode(timeParts[1]);

					string text = string.Join("\n", lines, 2, lines.Length - 2);

					subtitles.Add(new SubtitleEntry { startTime = start, endTime = end, text = text });
				}
			}
		}

		private float ParseTimecode(string timecode)
		{
			// Esempio: "00:01:20,500"
			TimeSpan time = TimeSpan.ParseExact(timecode.Trim(), @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
			return (float)time.TotalSeconds;
		}
	}
}