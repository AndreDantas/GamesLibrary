using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public static class SystemTranslations
{

    #region DEFAULT_MESSAGES
    public static readonly List<PairLanguageText> EXIT_TO_MAINMENU = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Ir para o menu principal?"),
    new PairLanguageText(SystemLanguage.English, "Go to main menu?") };

    public static readonly List<PairLanguageText> YES = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Sim"),
    new PairLanguageText(SystemLanguage.English, "Yes") };
    public static readonly List<PairLanguageText> NO = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "Não"),
    new PairLanguageText(SystemLanguage.English, "No") };



    #endregion

    #region ERROR_MESSAGES
    public static readonly List<PairLanguageText> EMPTY_INPUT_ERROR = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "O campo não pode estar vazio."),
    new PairLanguageText(SystemLanguage.English, "The field can't be empty.") };

    public static readonly List<PairLanguageText> INVALID_WORD_INPUT_ERROR = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "A palavra digitada é inválida."),
    new PairLanguageText(SystemLanguage.English, "The typed word is invalid.") };

    public static readonly List<PairLanguageText> REPEATED_WORD_INPUT_ERROR = new List<PairLanguageText> {
        new PairLanguageText(SystemLanguage.Portuguese, "A palavra digitada já existe."),
    new PairLanguageText(SystemLanguage.English, "The typed word already exists.") };

    #endregion
}
