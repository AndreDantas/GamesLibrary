using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using Sirenix.OdinInspector;

public class CatchphraseController : MonoBehaviour
{

    public Timer timer;
    public float countdown;
    public TextMeshProUGUI wordText;
    #region words
    string words = @"Adult

Aeroplane

Air

Aircraft Carrier

Airforce

Airport

Album

Alphabet

Apple

Arm

Army

Baby

Baby

Backpack

Balloon

Banana

Bank

Barbecue

Bathroom

Bathtub

Bed

Bed

Bee

Bible

Bible

Bird

Bomb

Book

Boss

Bottle

Bowl

Box

Boy

Brain

Bridge

Butterfly

Button

Cappuccino

Car

Car-race

Carpet

Carrot

Cave

Chair

Chess Board

Chief

Child

Chisel

Chocolates

Church

Church

Circle

Circus

Circus

Clock

Clown

Coffee

Coffee-shop

Comet

Compact Disc

Compass

Computer

Crystal

Cup

Cycle

Data Base

Desk

Diamond

Dress

Drill

Drink

Drum

Dung

Ears

Earth

Egg

Electricity

Elephant

Eraser

Explosive

Eyes

Family

Fan

Feather

Festival

Film

Finger

Fire

Floodlight

Flower

Foot

Fork

Freeway

Fruit

Fungus

Game

Garden

Gas

Gate

Gemstone

Girl

Gloves

God

Grapes

Guitar

Hammer

Hat

Hieroglyph

Highway

Horoscope

Horse

Hose

Ice

Ice-cream

Insect

Jet fighter

Junk

Kaleidoscope

Kitchen

Knife

Leather jacket

Leg

Library

Liquid

Magnet

Man

Map

Maze

Meat

Meteor

Microscope

Milk

Milkshake

Mist

Money

Monster

Mosquito

Mouth

Nail

Navy

Necklace

Needle

Onion

PaintBrush

Pants

Parachute

Passport

Pebble

Pendulum

Pepper

Perfume

Pillow

Plane

Planet

Pocket

Post-office

Potato

Printer

Prison

Pyramid

Radar

Rainbow

Record

Restaurant

Rifle

Ring

Robot

Rock

Rocket

Roof

Room

Rope

Saddle

Salt

Sandpaper

Sandwich

Satellite

School

Sex

Ship

Shoes

Shop

Shower

Signature

Skeleton

Slave

Snail

Software

Solid

Space Shuttle

Spectrum

Sphere

Spice

Spiral

Spoon

Sports-car

Spot Light

Square

Staircase

Star

Stomach

Sun

Sunglasses

Surveyor

Swimming Pool

Sword

Table

Tapestry

Teeth

Telescope

Television

Tennis racquet

Thermometer

Tiger

Toilet

Tongue

Torch

Torpedo

Train

Treadmill

Triangle

Tunnel

Typewriter

Umbrella

Vacuum

Vampire

Videotape

Vulture

Water

Weapon

Web

Wheelchair

Window

Woman

Worm

X-ray";
    #endregion
    public List<string> extraWords = new List<string>();
    List<string> currentWordList;
    [ShowInInspector]
    List<string> wordsToUse;
    public bool finished
    {
        get
        {
            if (timer)
                return timer.IsFinished();
            else
                return false;
        }
    }
    private void Awake()
    {
        LoadWords();

    }

    public void LoadWords()
    {
        currentWordList = new List<string>();
        foreach (var item in CatchphraseWordsFiles.instance.wordLists)
        {
            if (GameLanguage.language.systLanguage == item.language)
            {
                currentWordList = CatchphraseWordsFiles.GetWordsFromFile(item.wordList);
                ResetWordList();
                break;
            }
        }
        if (currentWordList.Count > 0)
            return;
        string[] temp = Regex.Split(words, @"\r?\n|\r");
        foreach (string s in temp)
        {
            if (s.Trim() == "")
                continue;
            currentWordList.Add(s.Trim().RemoveLineEndings());
        }
        ResetWordList();
        //NewWord();
    }

    public void ResetWordList()
    {
        wordsToUse = new List<string>(currentWordList);
        wordsToUse.AddRange(extraWords);
        for (int i = wordsToUse.Count - 1; i >= 0; i--)
        {
            if (wordsToUse[i].Trim() == "")
                wordsToUse.RemoveAt(i);
        }
        // wordsToUse.PrintAll();
    }



    public string NewWord()
    {
        if (wordsToUse.Count == 0)
            ResetWordList();
        int random = Random.Range(0, wordsToUse.Count);

        string word = wordsToUse[random];
        wordsToUse.RemoveAt(random);

        wordText.text = word.RemoveLineEndings();
        if (timer)
        {

            timer.gameObject.SetActive(true);
            timer.countdown = countdown;
            timer.StartTimer();
        }
        return word;
    }

    public void ShowTimer()
    {
        if (timer)
            timer.gameObject.SetActive(true);
    }

    public void HideTimer()
    {
        if (timer)
            timer.gameObject.SetActive(false);
    }

    public void Stop()
    {
        if (timer)
        {
            timer.StopTimer();
        }
    }

    public void Continue()
    {
        if (timer)
        {
            timer.ContinueTimer();
        }
    }
}
