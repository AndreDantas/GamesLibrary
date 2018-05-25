using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public abstract class Game : MonoBehaviour
{

    public static readonly List<PairLanguageText> EXIT_MATCH_CONFIRM = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Sair da Partida?"),
    new PairLanguageText(SystemLanguage.English, "Leave the match?") };

    public static readonly List<PairLanguageText> RESTART_MATCH_CONFIRM = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Reiniciar partida?"),
    new PairLanguageText(SystemLanguage.English, "Restart match?") };

    public static readonly List<PairLanguageText> GAME_SAVED = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Jogo Salvo."),
    new PairLanguageText(SystemLanguage.English, "Game saved.") };
    public static readonly List<PairLanguageText> LOAD_GAME_CONFIRM = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Carregar jogo salvo?"),
    new PairLanguageText(SystemLanguage.English, "Load saved game?") };

    public static readonly List<PairLanguageText> NO_GAME_SAVED = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Sem jogo salvo."),
    new PairLanguageText(SystemLanguage.English, "No saved game.") };

    public static readonly List<PairLanguageText> DRAW = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Empate"),
    new PairLanguageText(SystemLanguage.English, "Draw") };

    public static readonly List<PairLanguageText> WINNER = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Venceu"),
    new PairLanguageText(SystemLanguage.English, "Won") };

    public static readonly List<PairLanguageText> AI_NAME = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Computador"),
    new PairLanguageText(SystemLanguage.English, "AI") };

    public static readonly List<PairLanguageText> PLAYER_NAME = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Jogador"),
    new PairLanguageText(SystemLanguage.English, "Player") };




}
