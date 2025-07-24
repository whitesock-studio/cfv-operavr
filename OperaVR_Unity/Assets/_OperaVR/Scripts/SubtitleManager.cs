namespace OperaVR
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text.RegularExpressions;
	using UnityEngine;
	using UnityEngine.Events;

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
		[SerializeField] AudioSource _audioSource;
		[SerializeField] TextAsset _subtitleFile; // Inserisci qui il file .srt come TextAsset
		public SubtitleEvent OnSubtitleChanged = new SubtitleEvent();

		private List<SubtitleEntry> _subtitles = new List<SubtitleEntry>();
		private int _currentIndex = 0;
		private string _lastSubtitle = "";

		public void Awake()
		{
			OnSubtitleChanged.AddListener(DebugEntry);
			if (_audioSource == null || _subtitleFile == null)
			{
				Debug.LogError("SubtitleManager: AudioSource o SubtitleFile mancante.");
				return;
			}

			ParseSRT(_subtitleFile.text);
		}

		public void Play()
		{
			_currentIndex = 0;
			_lastSubtitle = "";
			_audioSource.Play();
			StartCoroutine(SubtitleCoroutine());
		}

		public void Stop()
		{
			_audioSource.Stop();
		}

		private void DebugEntry(string entry)
		{
			Debug.Log($"[SUBTITLE] {entry}");
		}

		private IEnumerator SubtitleCoroutine()
		{
			while (_audioSource.isPlaying)
			{
				float currentTime = _audioSource.time;
				if (_currentIndex < _subtitles.Count)
				{
					SubtitleEntry current = _subtitles[_currentIndex];
					if (currentTime >= current.startTime && currentTime <= current.endTime)
					{
						if (current.text != _lastSubtitle)
						{
							_lastSubtitle = current.text;
							OnSubtitleChanged.Invoke(current.text);
						}
					}
					else if (currentTime > current.endTime)
					{
						_currentIndex++;
						_lastSubtitle = "";
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
			_subtitles.Clear();
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

					_subtitles.Add(new SubtitleEntry { startTime = start, endTime = end, text = text });
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