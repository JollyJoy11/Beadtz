using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementUI : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Cancel()
    {
        SceneManager.LoadScene("Artists");
        DataCarrier.Instance.Reset();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Artists")
        {
            FindFirstObjectByType<ArtistUI>().PopulateArtists(GameManager.Instance.Player.ArtistsUnlocked);
        }
        else if (scene.name == "In Game")
        {
            FindFirstObjectByType<UIManager>().UpdatePlayerStats(GameManager.Instance.Player);
            FindFirstObjectByType<UIManager>().UpdateCityUI();

            if (GameManager.EvaluatingConcert != null && GameManager.EvaluatingConcert.Artist.IsOnBreak)
            {
                FindFirstObjectByType<ArtistBreakNotification>().ShowNotification(GameManager.EvaluatingConcert.Artist);
            }
        }
        else if (scene.name == "ArtistDetail")
        {
            FindFirstObjectByType<ArtistDetailUI>().DisplayArtistInfo(DataCarrier.Instance.SelectedArtist);
        }
        else if (scene.name == "PlanConcertSong")
        {
            FindFirstObjectByType<DisplaySongsUI>().PopulateSongs(DataCarrier.Instance.SelectedArtist, true);
        }
        else if (scene.name == "PlanConcertMap")
        {
            FindFirstObjectByType<UIManager>().UpdateCityUI();
            FindFirstObjectByType<UIManager>().UpdatePlayerStats(GameManager.Instance.Player);
            FindFirstObjectByType<UIManager>().UpdateClock(GameManager.Instance.CurrentGameTime);
        }
        else if (scene.name == "PlanConcertAttribute")
        {
            FindFirstObjectByType<UIManager>().DisplayMoneyAfterSelection(GameManager.Instance.Player);
            FindFirstObjectByType<UIManager>().UpdateClock(GameManager.Instance.CurrentGameTime);
            FindFirstObjectByType<ScheduleUI>().PopulateArtistSchedule(DataCarrier.Instance.SelectedArtist, GameManager.Instance.Player.Concerts);
            FindFirstObjectByType<ScheduleUI>().PopulateVenueSchedule(DataCarrier.Instance.SelectedVenue, GameManager.Instance.CurrentGameTime);
        }
        else if (scene.name == "ConcertEvaluation")
        {
            GameManager.Instance.Player.ViewConcertEvaluation(GameManager.EvaluatingConcert);
            FindFirstObjectByType<ConcertEvaluationUI>().DisplayConcertInfo(GameManager.EvaluatingConcert);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
