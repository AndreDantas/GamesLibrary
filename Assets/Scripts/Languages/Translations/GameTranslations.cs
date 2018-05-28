using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public static class GameTranslations
{


    #region GAME_MESSAGES

    public static readonly List<PairLanguageText> START = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Começar"),
    new PairLanguageText(SystemLanguage.English, "Start") };

    public static readonly List<PairLanguageText> RESTART = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Recomeçar"),
    new PairLanguageText(SystemLanguage.English, "Restart") };

    public static readonly List<PairLanguageText> PAUSED = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Pausado"),
    new PairLanguageText(SystemLanguage.English, "Paused") };

    public static readonly List<PairLanguageText> GAME_END = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Fim de jogo"),
    new PairLanguageText(SystemLanguage.English, "End of Match") };

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

    public static readonly List<PairLanguageText> PASSED = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Passou"),
    new PairLanguageText(SystemLanguage.English, "Passed") };

    public static readonly List<PairLanguageText> WON = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Venceu"),
    new PairLanguageText(SystemLanguage.English, "Won") };

    public static readonly List<PairLanguageText> WINNER = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Vencedor"),
    new PairLanguageText(SystemLanguage.English, "Winner") };

    public static readonly List<PairLanguageText> AI_NAME = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Computador"),
    new PairLanguageText(SystemLanguage.English, "AI") };

    public static readonly List<PairLanguageText> PLAYER_NAME = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Jogador"),
    new PairLanguageText(SystemLanguage.English, "Player") }

    ; public static readonly List<PairLanguageText> GAME_SPEED_FAST = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Rápido"),
    new PairLanguageText(SystemLanguage.English, "Fast") };

    public static readonly List<PairLanguageText> GAME_SPEED_NORMAL = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Normal"),
    new PairLanguageText(SystemLanguage.English, "Normal") };

    public static readonly List<PairLanguageText> GAME_SPEED_SLOW = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Lento"),
    new PairLanguageText(SystemLanguage.English, "Slow") };
    #endregion

    #region GAME_NAMES

    public static readonly List<PairLanguageText> CHESS = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Xadrez"),
    new PairLanguageText(SystemLanguage.English, "Chess") };
    public static readonly List<PairLanguageText> CHECKERS = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Damas"),
    new PairLanguageText(SystemLanguage.English, "Checkers") };
    public static readonly List<PairLanguageText> OTHELLO = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Othello"),
    new PairLanguageText(SystemLanguage.English, "Othello") };
    public static readonly List<PairLanguageText> WORDGUESS = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Adivinhação"),
    new PairLanguageText(SystemLanguage.English, "Word Guess") };
    public static readonly List<PairLanguageText> PONG = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Pong"),
    new PairLanguageText(SystemLanguage.English, "Pong") };
    public static readonly List<PairLanguageText> CONNECT = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Conectar"),
    new PairLanguageText(SystemLanguage.English, "Connect") };
    public static readonly List<PairLanguageText> DOTS_AND_BOXES = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Liga Pontos"),
    new PairLanguageText(SystemLanguage.English, "Dots & Boxes") };

    #endregion

}
