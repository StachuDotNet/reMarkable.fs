﻿module reMarkable.fs.UI.SegoeMdl2Glyphs

/// Glyph index table for the Segoe MDL2 Assets font
type Glyphs =
    | Accept = 0
    | AcceptMedium = 1
    | Accident = 2
    | AccidentSolid = 3
    | Accounts = 4
    | ActionCenter = 5
    | ActionCenterAsterisk = 6
    | ActionCenterMirrored = 7
    | ActionCenterNotification = 8
    | ActionCenterNotificationMirrored = 9
    | ActionCenterQuiet = 10
    | ActionCenterQuietNotification = 11
    | Add = 12
    | AddFriend = 13
    | AddNewLine = 14
    | AddNewLineFill = 15
    | AddRemoteDevice = 16
    | AddSurfaceHub = 17
    | AddTo = 18
    | AdjustHologram = 19
    | Admin = 20
    | Airplane = 21
    | AirplaneSolid = 22
    | AlignCenter = 23
    | AlignLeft = 24
    | AlignRight = 25
    | AllApps = 26
    | AllAppsMirrored = 27
    | Annotation = 28
    | AppIconDefault = 29
    | Apps = 30
    | AreaChart = 31
    | ArrowDown8 = 32
    | ArrowLeft8 = 33
    | ArrowRight8 = 34
    | ArrowUp8 = 35
    | AspectRatio = 36
    | Asterisk = 37
    | Attach = 38
    | AttachCamera = 39
    | Audio = 40
    | Back = 41
    | BackgroundToggle = 42
    | BackMirrored = 43
    | BackSpaceQWERTY = 44
    | BackToWindow = 45
    | Badge = 46
    | BandBattery0 = 47
    | BandBattery1 = 48
    | BandBattery2 = 49
    | BandBattery3 = 50
    | BandBattery4 = 51
    | BandBattery5 = 52
    | BandBattery6 = 53
    | Bank = 54
    | BarcodeScanner = 55
    | Battery0 = 56
    | Battery1 = 57
    | Battery10 = 58
    | Battery2 = 59
    | Battery3 = 60
    | Battery4 = 61
    | Battery5 = 62
    | Battery6 = 63
    | Battery7 = 64
    | Battery8 = 65
    | Battery9 = 66
    | BatteryCharging0 = 67
    | BatteryCharging1 = 68
    | BatteryCharging10 = 69
    | BatteryCharging2 = 70
    | BatteryCharging3 = 71
    | BatteryCharging4 = 72
    | BatteryCharging5 = 73
    | BatteryCharging6 = 74
    | BatteryCharging7 = 75
    | BatteryCharging8 = 76
    | BatteryCharging9 = 77
    | BatterySaver0 = 78
    | BatterySaver1 = 79
    | BatterySaver10 = 80
    | BatterySaver2 = 81
    | BatterySaver3 = 82
    | BatterySaver4 = 83
    | BatterySaver5 = 84
    | BatterySaver6 = 85
    | BatterySaver7 = 86
    | BatterySaver8 = 87
    | BatterySaver9 = 88
    | BatteryUnknown = 89
    | Beta = 90
    | BidiLtr = 91
    | BidiRtl = 92
    | BlockContact = 93
    | Blocked2 = 94
    | BlueLight = 95
    | Bluetooth = 96
    | BodyCam = 97
    | Bold = 98
    | Bookmarks = 99
    | BookmarksMirrored = 100
    | Brightness = 101
    | Broom = 102
    | BrowsePhotos = 103
    | BrushSize = 104
    | Bug = 105
    | BuildingEnergy = 106
    | BulletedList = 107
    | BulletedListMirrored = 108
    | Bullseye = 109
    | BumperLeft = 110
    | BumperRight = 111
    | Bus = 112
    | BusSolid = 113
    | ButtonA = 114
    | ButtonB = 115
    | ButtonMenu = 116
    | ButtonView2 = 117
    | ButtonX = 118
    | ButtonY = 119
    | Cafe = 120
    | Calculator = 121
    | CalculatorAddition = 122
    | CalculatorBackspace = 123
    | CalculatorDivide = 124
    | CalculatorEqualTo = 125
    | CalculatorMultiply = 126
    | CalculatorNegate = 127
    | CalculatorPercentage = 128
    | CalculatorSquareroot = 129
    | CalculatorSubtract = 130
    | Calendar = 131
    | CalendarDay = 132
    | CalendarMirrored = 133
    | CalendarReply = 134
    | CalendarSolid = 135
    | CalendarWeek = 136
    | CallControl = 137
    | CallForwarding = 138
    | CallForwardingMirrored = 139
    | CallForwardInternational = 140
    | CallForwardInternationalMirrored = 141
    | CallForwardRoaming = 142
    | CallForwardRoamingMirrored = 143
    | CalligraphyFill = 144
    | CalligraphyPen = 145
    | Calories = 146
    | Camera = 147
    | Cancel = 148
    | CancelMedium = 149
    | Caption = 150
    | Car = 151
    | CaretBottomRightSolidCenter8 = 152
    | CaretDownSolid8 = 153
    | CaretLeftSolid8 = 154
    | CaretRight8 = 155
    | CaretRightSolid8 = 156
    | CaretUpSolid8 = 157
    | CashDrawer = 158
    | CC = 159
    | CellPhone = 160
    | Certificate = 161
    | CharacterAppearance = 162
    | Characters = 163
    | ChatBubbles = 164
    | Checkbox = 165
    | CheckboxComposite = 166
    | CheckboxCompositeReversed = 167
    | CheckboxFill = 168
    | CheckboxIndeterminate = 169
    | CheckboxIndeterminateCombo = 170
    | CheckList = 171
    | ChecklistMirrored = 172
    | CheckMark = 173
    | ChevronDown = 174
    | ChevronLeft = 175
    | ChevronRight = 176
    | ChevronUp = 177
    | ChineseBoPoMoFo = 178
    | ChineseChangjie = 179
    | ChinesePinyin = 180
    | ChineseQuick = 181
    | ChipCardCreditCardReader = 182
    | CHTLanguageBar = 183
    | CircleFill = 184
    | CircleFillBadge12 = 185
    | CircleRing = 186
    | CircleRingBadge12 = 187
    | CityNext = 188
    | CityNext2 = 189
    | Clear = 190
    | ClearAllInk = 191
    | ClearAllInkMirrored = 192
    | ClearSelection = 193
    | ClearSelectionMirrored = 194
    | Click = 195
    | ClipboardList = 196
    | ClipboardListMirrored = 197
    | ClippingTool = 198
    | ClosePane = 199
    | ClosePaneMirrored = 200
    | Cloud = 201
    | CloudPrinter = 202
    | CloudSearch = 203
    | Code = 204
    | CollapseContent = 205
    | CollapseContentSingle = 206
    | CollateLandscape = 207
    | CollateLandscapeSeparated = 208
    | CollatePortrait = 209
    | CollatePortraitSeparated = 210
    | Color = 211
    | ColorOff = 212
    | CommandPrompt = 213
    | Comment = 214
    | Communications = 215
    | CompanionApp = 216
    | CompanionDeviceFramework = 217
    | Completed = 218
    | CompletedSolid = 219
    | Component = 220
    | Connect = 221
    | ConnectApp = 222
    | Connected = 223
    | Construction = 224
    | ConstructionCone = 225
    | ConstructionSolid = 226
    | Contact = 227
    | Contact2 = 228
    | ContactInfo = 229
    | ContactInfoMirrored = 230
    | ContactPresence = 231
    | ContactSolid = 232
    | Copy = 233
    | CopyTo = 234
    | Courthouse = 235
    | Crop = 236
    | CtrlSpatialLeft = 237
    | CtrlSpatialRight = 238
    | Cut = 239
    | DataSense = 240
    | DataSenseBar = 241
    | DateTime = 242
    | DateTimeMirrored = 243
    | DefaultAPN = 244
    | DefenderApp = 245
    | Delete = 246
    | DeleteLines = 247
    | DeleteLinesFill = 248
    | DeleteWord = 249
    | DeleteWordFill = 250
    | DeliveryOptimization = 251
    | Design = 252
    | DetachablePC = 253
    | DeveloperTools = 254
    | DeviceDiscovery = 255
    | DeviceLaptopNoPic = 256
    | DeviceLaptopPic = 257
    | DeviceMonitorLeftPic = 258
    | DeviceMonitorNoPic = 259
    | DeviceMonitorRightPic = 260
    | Devices = 261
    | Devices2 = 262
    | Devices3 = 263
    | Devices4 = 264
    | DevUpdate = 265
    | Diagnostic = 266
    | Dialpad = 267
    | DialShape1 = 268
    | DialShape2 = 269
    | DialShape3 = 270
    | DialShape4 = 271
    | DialUp = 272
    | Dictionary = 273
    | DictionaryAdd = 274
    | DictionaryCloud = 275
    | DirectAccess = 276
    | Directions = 277
    | DisableUpdates = 278
    | DisconnectDisplay = 279
    | DisconnectDrive = 280
    | Dislike = 281
    | DMC = 282
    | Dock = 283
    | DockBottom = 284
    | DockLeft = 285
    | DockLeftMirrored = 286
    | DockRight = 287
    | DockRightMirrored = 288
    | Document = 289
    | DoubleLandscape = 290
    | DoublePinyin = 291
    | DoublePortrait = 292
    | Down = 293
    | Download = 294
    | DownloadMap = 295
    | Dpad = 296
    | Draw = 297
    | DrawSolid = 298
    | DrivingMode = 299
    | Drop = 300
    | DullSound = 301
    | DuplexLandscapeOneSided = 302
    | DuplexLandscapeOneSidedMirrored = 303
    | DuplexLandscapeTwoSidedLongEdge = 304
    | DuplexLandscapeTwoSidedLongEdgeMirrored = 305
    | DuplexLandscapeTwoSidedShortEdge = 306
    | DuplexLandscapeTwoSidedShortEdgeMirrored = 307
    | DuplexPortraitOneSided = 308
    | DuplexPortraitOneSidedMirrored = 309
    | DuplexPortraitTwoSidedLongEdge = 310
    | DuplexPortraitTwoSidedLongEdgeMirrored = 311
    | DuplexPortraitTwoSidedShortEdge = 312
    | DuplexPortraitTwoSidedShortEdgeMirrored = 313
    | DynamicLock = 314
    | Ear = 315
    | Earbud = 316
    | EaseOfAccess = 317
    | Edit = 318
    | EditMirrored = 319
    | Education = 320
    | EducationIcon = 321
    | Eject = 322
    | EMI = 323
    | Emoji = 324
    | Emoji2 = 325
    | EmojiTabCelebrationObjects = 326
    | EmojiTabFavorites = 327
    | EmojiTabFoodPlants = 328
    | EmojiTabPeople = 329
    | EmojiTabSmilesAnimals = 330
    | EmojiTabSymbols = 331
    | EmojiTabTextSmiles = 332
    | EmojiTabTransitPlaces = 333
    | EndPoint = 334
    | EndPointSolid = 335
    | Equalizer = 336
    | EraseTool = 337
    | EraseToolFill = 338
    | EraseToolFill2 = 339
    | Error = 340
    | ErrorBadge = 341
    | ErrorBadge12 = 342
    | eSIM = 343
    | eSIMBusy = 344
    | eSIMLocked = 345
    | eSIMNoProfile = 346
    | Ethernet = 347
    | EthernetError = 348
    | EthernetWarning = 349
    | ExpandTile = 350
    | ExpandTileMirrored = 351
    | ExploitProtectionSettings = 352
    | ExploreContent = 353
    | ExploreContentSingle = 354
    | Export = 355
    | ExportMirrored = 356
    | Eyedropper = 357
    | EyeGaze = 358
    | Family = 359
    | FastForward = 360
    | Favicon = 361
    | Favicon2 = 362
    | FavoriteList = 363
    | FavoriteStar = 364
    | FavoriteStarFill = 365
    | Feedback = 366
    | FeedbackApp = 367
    | Ferry = 368
    | FerrySolid = 369
    | FileExplorer = 370
    | FileExplorerApp = 371
    | Filter = 372
    | FingerInking = 373
    | Fingerprint = 374
    | FitPage = 375
    | Flag = 376
    | Flashlight = 377
    | FlickDown = 378
    | FlickLeft = 379
    | FlickRight = 380
    | FlickUp = 381
    | Flow = 382
    | Folder = 383
    | FolderFill = 384
    | FolderHorizontal = 385
    | FolderOpen = 386
    | Font = 387
    | FontColor = 388
    | FontDecrease = 389
    | FontIncrease = 390
    | FontSize = 391
    | Forward = 392
    | ForwardCall = 393
    | ForwardMirrored = 394
    | ForwardSm = 395
    | FourBars = 396
    | FreeFormClipping = 397
    | Frigid = 398
    | FullAlpha = 399
    | FullHiragana = 400
    | FullKatakana = 401
    | FullScreen = 402
    | FuzzyReading = 403
    | Game = 404
    | GameConsole = 405
    | GenericScan = 406
    | GIF = 407
    | GiftboxOpen = 408
    | GlobalNavigationButton = 409
    | Globe = 410
    | Globe2 = 411
    | Go = 412
    | GoMirrored = 413
    | GoToMessage = 414
    | GoToStart = 415
    | GotoToday = 416
    | GridView = 417
    | GripperBarHorizontal = 418
    | GripperBarVertical = 419
    | GripperResize = 420
    | GripperResizeMirrored = 421
    | GripperTool = 422
    | Groceries = 423
    | Group = 424
    | GroupList = 425
    | GuestUser = 426
    | HalfAlpha = 427
    | HalfDullSound = 428
    | HalfKatakana = 429
    | HalfStarLeft = 430
    | HalfStarRight = 431
    | Handwriting = 432
    | HangUp = 433
    | HardDrive = 434
    | HeadlessDevice = 435
    | Headphone = 436
    | Headphone0 = 437
    | Headphone1 = 438
    | Headphone2 = 439
    | Headphone3 = 440
    | Headset = 441
    | Health = 442
    | Heart = 443
    | HeartBroken = 444
    | HeartFill = 445
    | Help = 446
    | HelpMirrored = 447
    | HideBcc = 448
    | Highlight = 449
    | HighlightFill = 450
    | HighlightFill2 = 451
    | History = 452
    | HMD = 453
    | HoloLensSelected = 454
    | Home = 455
    | HomeGroup = 456
    | HomeSolid = 457
    | HWPInsert = 458
    | HWPJoin = 459
    | HWPNewLine = 460
    | HWPOverwrite = 461
    | HWPScratchOut = 462
    | HWPSplit = 463
    | HWPStrikeThrough = 464
    | IBeam = 465
    | IBeamOutline = 466
    | ImageExport = 467
    | Import = 468
    | ImportAll = 469
    | ImportAllMirrored = 470
    | Important = 471
    | ImportMirrored = 472
    | IncidentTriangle = 473
    | IncomingCall = 474
    | Info = 475
    | Info2 = 476
    | InfoSolid = 477
    | InkingCaret = 478
    | InkingColorFill = 479
    | InkingColorOutline = 480
    | InkingTool = 481
    | InkingToolFill = 482
    | InkingToolFill2 = 483
    | InPrivate = 484
    | Input = 485
    | InsiderHubApp = 486
    | InstertWords = 487
    | InstertWordsFill = 488
    | InteractiveDashboard = 489
    | InternetSharing = 490
    | IOT = 491
    | Italic = 492
    | Japanese = 493
    | JoinWords = 494
    | JoinWordsFill = 495
    | JpnRomanji = 496
    | JpnRomanjiLock = 497
    | JpnRomanjiShift = 498
    | JpnRomanjiShiftLock = 499
    | Key12On = 500
    | Keyboard12Key = 501
    | KeyboardBrightness = 502
    | KeyboardClassic = 503
    | KeyboardDismiss = 504
    | KeyboardDock = 505
    | KeyboardFull = 506
    | KeyboardLeftAligned = 507
    | KeyboardLeftDock = 508
    | KeyboardLeftHanded = 509
    | KeyboardLowerBrightness = 510
    | KeyboardNarrow = 511
    | KeyboardOneHanded = 512
    | KeyboardRightAligned = 513
    | KeyboardRightDock = 514
    | KeyboardRightHanded = 515
    | KeyboardSettings = 516
    | KeyboardShortcut = 517
    | KeyboardSplit = 518
    | KeyboardStandard = 519
    | KeyboardUndock = 520
    | Kiosk = 521
    | KnowledgeArticle = 522
    | Korean = 523
    | Label = 524
    | LandscapeOrientation = 525
    | LandscapeOrientationMirrored = 526
    | LangJPN = 527
    | LanguageChs = 528
    | LanguageCht = 529
    | LanguageJpn = 530
    | LanguageKor = 531
    | LaptopSecure = 532
    | LaptopSelected = 533
    | LargeErase = 534
    | Leaf = 535
    | LeaveChat = 536
    | LeaveChatMirrored = 537
    | LEDLight = 538
    | LeftArrowKeyTime0 = 539
    | LeftDoubleQuote = 540
    | LeftQuote = 541
    | LeftStick = 542
    | Lexicon = 543
    | Library = 544
    | Light = 545
    | Lightbulb = 546
    | LightningBolt = 547
    | Like = 548
    | LikeDislike = 549
    | LineDisplay = 550
    | Link = 551
    | List = 552
    | ListMirrored = 553
    | LocaleLanguage = 554
    | Location = 555
    | Lock = 556
    | LockFeedback = 557
    | LockscreenDesktop = 558
    | LockScreenGlance = 559
    | LowerBrightness = 560
    | MagStripeReader = 561
    | Mail = 562
    | MailFill = 563
    | MailForward = 564
    | MailForwardMirrored = 565
    | MailReply = 566
    | MailReplyAll = 567
    | MailReplyAllMirrored = 568
    | MailReplyMirrored = 569
    | Manage = 570
    | MapCompassBottom = 571
    | MapCompassTop = 572
    | MapDirections = 573
    | MapDrive = 574
    | MapLayers = 575
    | MapPin = 576
    | MapPin2 = 577
    | Marker = 578
    | Marquee = 579
    | Media = 580
    | MediaStorageTower = 581
    | Megaphone = 582
    | Memo = 583
    | MergeCall = 584
    | Message = 585
    | MicClipping = 586
    | MicError = 587
    | MicOff = 588
    | MicOff2 = 589
    | MicOn = 590
    | Microphone = 591
    | MicrophoneListening = 592
    | MicSleep = 593
    | MiracastLogoLarge = 594
    | MiracastLogoSmall = 595
    | MixedMediaBadge = 596
    | MixVolumes = 597
    | More = 598
    | Mouse = 599
    | MoveToFolder = 600
    | Movies = 601
    | MultimediaDMP = 602
    | MultimediaDMS = 603
    | MultimediaDVR = 604
    | MultimediaPMP = 605
    | MultiSelect = 606
    | MultiSelectMirrored = 607
    | MusicAlbum = 608
    | MusicInfo = 609
    | MusicNote = 610
    | MusicSharing = 611
    | MusicSharingOff = 612
    | Mute = 613
    | MyNetwork = 614
    | Narrator = 615
    | NarratorApp = 616
    | NarratorForward = 617
    | NarratorForwardMirrored = 618
    | NearbySharing = 619
    | Network = 620
    | NetworkAdapter = 621
    | NetworkConnected = 622
    | NetworkConnectedCheckmark = 623
    | NetworkOffline = 624
    | NetworkPrinter = 625
    | NetworkSharing = 626
    | NetworkTower = 627
    | NewFolder = 628
    | NewWindow = 629
    | Next = 630
    | NoiseCancelation = 631
    | NoiseCancelationOff = 632
    | NUIFace = 633
    | NUIIris = 634
    | OEM = 635
    | OneBar = 636
    | OpenFile = 637
    | OpenFolderHorizontal = 638
    | OpenInNewWindow = 639
    | OpenLocal = 640
    | OpenPane = 641
    | OpenPaneMirrored = 642
    | OpenWith = 643
    | OpenWithMirrored = 644
    | Orientation = 645
    | OtherUser = 646
    | OutlineHalfStarLeft = 647
    | OutlineHalfStarRight = 648
    | OutlineQuarterStarLeft = 649
    | OutlineQuarterStarRight = 650
    | OutlineStarLeftHalf = 651
    | OutlineStarRightHalf = 652
    | OutlineThreeQuarterStarLeft = 653
    | OutlineThreeQuarterStarRight = 654
    | OverwriteWords = 655
    | OverwriteWordsFill = 656
    | OverwriteWordsFillKorean = 657
    | OverwriteWordsKorean = 658
    | Package = 659
    | Page = 660
    | PageLeft = 661
    | PageMarginLandscapeModerate = 662
    | PageMarginLandscapeNarrow = 663
    | PageMarginLandscapeNormal = 664
    | PageMarginLandscapeWide = 665
    | PageMarginPortraitModerate = 666
    | PageMarginPortraitNarrow = 667
    | PageMarginPortraitNormal = 668
    | PageMarginPortraitWide = 669
    | PageMirrored = 670
    | PageRight = 671
    | PageSolid = 672
    | PanMode = 673
    | ParkingLocation = 674
    | ParkingLocationMirrored = 675
    | ParkingLocationSolid = 676
    | PartyLeader = 677
    | PassiveAuthentication = 678
    | PasswordKeyHide = 679
    | PasswordKeyShow = 680
    | Paste = 681
    | Pause = 682
    | PaymentCard = 683
    | PC1 = 684
    | PDF = 685
    | Pencil = 686
    | PencilFill = 687
    | PenPalette = 688
    | PenPaletteMirrored = 689
    | PenTips = 690
    | PenTipsMirrored = 691
    | PenWorkspace = 692
    | PenWorkspaceMirrored = 693
    | People = 694
    | Permissions = 695
    | PersonalFolder = 696
    | Personalize = 697
    | Phone = 698
    | PhoneBook = 699
    | Photo = 700
    | Photo2 = 701
    | Picture = 702
    | PieSingle = 703
    | Pin = 704
    | PinFill = 705
    | Pinned = 706
    | PinnedFill = 707
    | PINPad = 708
    | PLAP = 709
    | Play = 710
    | PlaybackRate1x = 711
    | PlaybackRateOther = 712
    | PlayerSettings = 713
    | PlaySolid = 714
    | PointErase = 715
    | PointEraseMirrored = 716
    | PointerHand = 717
    | PoliceCar = 718
    | PostUpdate = 719
    | PowerButton = 720
    | PowerButtonUpdate = 721
    | PPSFourLandscape = 722
    | PPSFourPortrait = 723
    | PPSOneLandscape = 724
    | PPSOnePortrait = 725
    | PPSTwoLandscape = 726
    | PPSTwoPortrait = 727
    | PresenceChicklet = 728
    | PresenceChickletVideo = 729
    | Preview = 730
    | PreviewLink = 731
    | Previous = 732
    | Print = 733
    | PrintAllPages = 734
    | PrintCustomRange = 735
    | PrintDefault = 736
    | Printer3D = 737
    | PrintfaxPrinterFile = 738
    | Priority = 739
    | PrivateCall = 740
    | Process = 741
    | Processing = 742
    | ProductivityMode = 743
    | ProgressRingDots = 744
    | Project = 745
    | Projector = 746
    | ProtectedDocument = 747
    | Protractor = 748
    | ProvisioningPackage = 749
    | Puzzle = 750
    | QRCode = 751
    | QuarentinedItems = 752
    | QuarentinedItemsMirrored = 753
    | QuarterStarLeft = 754
    | QuarterStarRight = 755
    | QuickNote = 756
    | QuietHours = 757
    | QWERTYOff = 758
    | QWERTYOn = 759
    | Radar = 760
    | RadioBtnOff = 761
    | RadioBtnOn = 762
    | RadioBullet = 763
    | RadioBullet2 = 764
    | Read = 765
    | ReadingList = 766
    | ReadingMode = 767
    | ReceiptPrinter = 768
    | Recent = 769
    | Record = 770
    | Record2 = 771
    | RectangularClipping = 772
    | RedEye = 773
    | Redo = 774
    | Refresh = 775
    | Relationship = 776
    | RememberedDevice = 777
    | Reminder = 778
    | ReminderFill = 779
    | Remote = 780
    | Remove = 781
    | RemoveFrom = 782
    | Rename = 783
    | Repair = 784
    | RepeatAll = 785
    | RepeatOff = 786
    | RepeatOne = 787
    | Replay = 788
    | Reply = 789
    | ReplyMirrored = 790
    | ReportDocument = 791
    | ReportHacked = 792
    | ResetDevice = 793
    | ResetDrive = 794
    | Reshare = 795
    | ResizeMouseLarge = 796
    | ResizeMouseMedium = 797
    | ResizeMouseMediumMirrored = 798
    | ResizeMouseSmall = 799
    | ResizeMouseSmallMirrored = 800
    | ResizeMouseTall = 801
    | ResizeMouseTallMirrored = 802
    | ResizeMouseWide = 803
    | ResizeTouchLarger = 804
    | ResizeTouchNarrower = 805
    | ResizeTouchNarrowerMirrored = 806
    | ResizeTouchShorter = 807
    | ResizeTouchSmaller = 808
    | RestartUpdate = 809
    | ReturnKeyLg = 810
    | ReturnKeySm = 811
    | ReturnToCall = 812
    | ReturnToWindow = 813
    | RevealPasswordMedium = 814
    | Rewind = 815
    | RightArrowKeyTime0 = 816
    | RightArrowKeyTime1 = 817
    | RightArrowKeyTime2 = 818
    | RightArrowKeyTime3 = 819
    | RightArrowKeyTime4 = 820
    | RightDoubleQuote = 821
    | RightQuote = 822
    | RightStick = 823
    | Ringer = 824
    | RingerSilent = 825
    | RoamingDomestic = 826
    | RoamingInternational = 827
    | Robot = 828
    | Rotate = 829
    | RotateCamera = 830
    | RotateMapLeft = 831
    | RotateMapRight = 832
    | RotationLock = 833
    | RTTLogo = 834
    | Ruler = 835
    | Safe = 836
    | Save = 837
    | SaveAs = 838
    | SaveCopy = 839
    | SaveLocal = 840
    | Scan = 841
    | ScreenTime = 842
    | ScrollMode = 843
    | ScrollUpDown = 844
    | SDCard = 845
    | Search = 846
    | SearchAndApps = 847
    | SearchMedium = 848
    | SelectAll = 849
    | Send = 850
    | SendFill = 851
    | SendFillMirrored = 852
    | SendMirrored = 853
    | Sensor = 854
    | Set = 855
    | SetHistoryStatus = 856
    | SetHistoryStatus2 = 857
    | SetlockScreen = 858
    | SetSolid = 859
    | SetTile = 860
    | Setting = 861
    | SettingsBattery = 862
    | SettingsDisplaySound = 863
    | Share = 864
    | ShareBroadband = 865
    | Shield = 866
    | Shop = 867
    | ShoppingCart = 868
    | ShowBcc = 869
    | ShowResults = 870
    | ShowResultsMirrored = 871
    | Shuffle = 872
    | SignalBars1 = 873
    | SignalBars2 = 874
    | SignalBars3 = 875
    | SignalBars4 = 876
    | SignalBars5 = 877
    | SignalError = 878
    | SignalNotConnected = 879
    | SignalRoaming = 880
    | SignatureCapture = 881
    | SignOut = 882
    | SIMError = 883
    | SIMLock = 884
    | SIMMissing = 885
    | SingleLandscape = 886
    | SinglePortrait = 887
    | SIPMove = 888
    | SIPRedock = 889
    | SIPUndock = 890
    | SkipBack10 = 891
    | SkipForward30 = 892
    | SliderThumb = 893
    | Slideshow = 894
    | SlowMotionOn = 895
    | SmallErase = 896
    | Smartcard = 897
    | SmartcardVirtual = 898
    | Sort = 899
    | SpatialVolume0 = 900
    | SpatialVolume1 = 901
    | SpatialVolume2 = 902
    | SpatialVolume3 = 903
    | Speakers = 904
    | Speech = 905
    | SpeedHigh = 906
    | SpeedMedium = 907
    | SpeedOff = 908
    | StaplingLandscapeBookBinding = 909
    | StaplingLandscapeBottomLeft = 910
    | StaplingLandscapeBottomRight = 911
    | StaplingLandscapeTopLeft = 912
    | StaplingLandscapeTopRight = 913
    | StaplingLandscapeTwoBottom = 914
    | StaplingLandscapeTwoLeft = 915
    | StaplingLandscapeTwoRight = 916
    | StaplingLandscapeTwoTop = 917
    | StaplingOff = 918
    | StaplingPortraitBookBinding = 919
    | StaplingPortraitBottomLeft = 920
    | StaplingPortraitBottomRight = 921
    | StaplingPortraitTopLeft = 922
    | StaplingPortraitTopRight = 923
    | StaplingPortraitTwoBottom = 924
    | StaplingPortraitTwoLeft = 925
    | StaplingPortraitTwoRight = 926
    | StaplingPortraitTwoTop = 927
    | StartPoint = 928
    | StartPointSolid = 929
    | StartPresenting = 930
    | StatusCheckmark = 931
    | StatusCheckmarkLeft = 932
    | StatusCircle = 933
    | StatusCircleBlock = 934
    | StatusCircleBlock2 = 935
    | StatusCircleCheckmark = 936
    | StatusCircleErrorX = 937
    | StatusCircleExclamation = 938
    | StatusCircleInfo = 939
    | StatusCircleInner = 940
    | StatusCircleLeft = 941
    | StatusCircleOuter = 942
    | StatusCircleQuestionMark = 943
    | StatusCircleRing = 944
    | StatusCircleSync = 945
    | StatusConnecting1 = 946
    | StatusConnecting2 = 947
    | StatusDataTransfer = 948
    | StatusDataTransferRoaming = 949
    | StatusDataTransferVPN = 950
    | StatusDualSIM1 = 951
    | StatusDualSIM1VPN = 952
    | StatusDualSIM2 = 953
    | StatusDualSIM2VPN = 954
    | StatusError = 955
    | StatusErrorFull = 956
    | StatusErrorLeft = 957
    | StatusInfo = 958
    | StatusInfoLeft = 959
    | StatusSecured = 960
    | StatusSGLTE = 961
    | StatusSGLTECell = 962
    | StatusSGLTEDataVPN = 963
    | StatusTriangle = 964
    | StatusTriangleExclamation = 965
    | StatusTriangleInner = 966
    | StatusTriangleLeft = 967
    | StatusTriangleOuter = 968
    | StatusUnsecure = 969
    | StatusVPN = 970
    | StatusWarning = 971
    | StatusWarningLeft = 972
    | Sticker2 = 973
    | StockDown = 974
    | StockUp = 975
    | Stop = 976
    | StopPoint = 977
    | StopPointSolid = 978
    | StopPresenting = 979
    | Stopwatch = 980
    | StorageNetworkWireless = 981
    | StorageOptical = 982
    | StorageTape = 983
    | Streaming = 984
    | StreamingEnterprise = 985
    | Street = 986
    | StreetsideSplitExpand = 987
    | StreetsideSplitMinimize = 988
    | StrokeErase = 989
    | StrokeErase2 = 990
    | StrokeEraseMirrored = 991
    | SubscriptionAdd = 992
    | SubscriptionAddMirrored = 993
    | Subtitles = 994
    | SubtitlesAudio = 995
    | SurfaceHub = 996
    | SurfaceHubSelected = 997
    | Sustainable = 998
    | Swipe = 999
    | Switch = 1000
    | SwitchApps = 1001
    | SwitchUser = 1002
    | Sync = 1003
    | SyncError = 1004
    | SyncFolder = 1005
    | System = 1006
    | Tablet = 1007
    | TabletMode = 1008
    | TabletSelected = 1009
    | Tag = 1010
    | TapAndSend = 1011
    | TaskbarPhone = 1012
    | TaskView = 1013
    | TaskViewExpanded = 1014
    | TaskViewSettings = 1015
    | ThisPC = 1016
    | ThoughtBubble = 1017
    | ThreeBars = 1018
    | ThreeQuarterStarLeft = 1019
    | ThreeQuarterStarRight = 1020
    | Tiles = 1021
    | TiltDown = 1022
    | TiltUp = 1023
    | TimeLanguage = 1024
    | ToggleBorder = 1025
    | ToggleFilled = 1026
    | ToggleThumb = 1027
    | TollSolid = 1028
    | ToolTip = 1029
    | Touch = 1030
    | Touchpad = 1031
    | TouchPointer = 1032
    | Touchscreen = 1033
    | Trackers = 1034
    | TrackersMirrored = 1035
    | TrafficCongestionSolid = 1036
    | TrafficLight = 1037
    | Train = 1038
    | TrainSolid = 1039
    | TreeFolderFolder = 1040
    | TreeFolderFolderFill = 1041
    | TreeFolderFolderOpen = 1042
    | TreeFolderFolderOpenFill = 1043
    | TriggerLeft = 1044
    | TriggerRight = 1045
    | Trim = 1046
    | TVMonitor = 1047
    | TVMonitorSelected = 1048
    | TwoBars = 1049
    | TwoPage = 1050
    | Type = 1051
    | Underline = 1052
    | UnderscoreSpace = 1053
    | Undo = 1054
    | Unfavorite = 1055
    | Unit = 1056
    | Unknown = 1057
    | UnknownMirrored = 1058
    | Unlock = 1059
    | Unpin = 1060
    | UnsyncFolder = 1061
    | Up = 1062
    | UpdateRestore = 1063
    | UpdateStatusDot = 1064
    | Upload = 1065
    | USB = 1066
    | USBSafeConnect = 1067
    | UserAPN = 1068
    | VerticalBattery0 = 1069
    | VerticalBattery1 = 1070
    | VerticalBattery10 = 1071
    | VerticalBattery2 = 1072
    | VerticalBattery3 = 1073
    | VerticalBattery4 = 1074
    | VerticalBattery5 = 1075
    | VerticalBattery6 = 1076
    | VerticalBattery7 = 1077
    | VerticalBattery8 = 1078
    | VerticalBattery9 = 1079
    | VerticalBatteryCharging0 = 1080
    | VerticalBatteryCharging1 = 1081
    | VerticalBatteryCharging10 = 1082
    | VerticalBatteryCharging2 = 1083
    | VerticalBatteryCharging3 = 1084
    | VerticalBatteryCharging4 = 1085
    | VerticalBatteryCharging5 = 1086
    | VerticalBatteryCharging6 = 1087
    | VerticalBatteryCharging7 = 1088
    | VerticalBatteryCharging8 = 1089
    | VerticalBatteryCharging9 = 1090
    | VerticalBatteryUnknown = 1091
    | Vibrate = 1092
    | Video = 1093
    | Video360 = 1094
    | VideoCapture = 1095
    | VideoChat = 1096
    | VideoSolid = 1097
    | View = 1098
    | ViewAll = 1099
    | ViewDashboard = 1100
    | VirtualMachineGroup = 1101
    | VoiceCall = 1102
    | Volume = 1103
    | Volume0 = 1104
    | Volume1 = 1105
    | Volume2 = 1106
    | Volume3 = 1107
    | VolumeBars = 1108
    | VPN = 1109
    | Walk = 1110
    | WalkSolid = 1111
    | Warning = 1112
    | Webcam = 1113
    | Webcam2 = 1114
    | WebSearch = 1115
    | Website = 1116
    | Wheel = 1117
    | Wifi1 = 1118
    | Wifi2 = 1119
    | Wifi3 = 1120
    | Wifi4 = 1121
    | WifiAttentionOverlay = 1122
    | WifiCall0 = 1123
    | WifiCall1 = 1124
    | WifiCall2 = 1125
    | WifiCall3 = 1126
    | WifiCall4 = 1127
    | WifiCallBars = 1128
    | WifiError0 = 1129
    | WifiError1 = 1130
    | WifiError2 = 1131
    | WifiError3 = 1132
    | WifiError4 = 1133
    | WifiEthernet = 1134
    | WifiHotspot = 1135
    | WifiWarning0 = 1136
    | WifiWarning1 = 1137
    | WifiWarning2 = 1138
    | WifiWarning3 = 1139
    | WifiWarning4 = 1140
    | WindDirection = 1141
    | WindowsInsider = 1142
    | WindowSnipping = 1143
    | Wire = 1144
    | WiredUSB = 1145
    | WirelessUSB = 1146
    | Work = 1147
    | WorkSolid = 1148
    | World = 1149
    | XboxOneConsole = 1150
    | ZeroBars = 1151
    | Zoom = 1152
    | ZoomIn = 1153
    | ZoomMode = 1154
    | ZoomOut = 1155
    
open System
open System.Collections.Generic
open System.Linq
open System.Reflection
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.ResourceHelpers

type SymbolAtlasPage(imagePage: byte[], glyphSize: int) =
    member _.Page: Image<Rgba32> = Image.Load(imagePage)
    
    member val GlyphSize = glyphSize with get, set

type SymbolAtlas<'T when 'T : enum<int>>(pages: SymbolAtlasPage[]) =
    let pages: Dictionary<int, Image<Rgba32>> =
        let a = fun (pg: SymbolAtlasPage) -> pg.GlyphSize
        let b = fun (pg: SymbolAtlasPage) -> pg.Page
        pages.ToDictionary(a, b)

    member _.AvailableSizes: IEnumerable<int> =
        pages |> Seq.map(fun z -> z.Key)

    member this.GetIcon(size: int, glyph: 'T): Image<Rgba32> =
        match pages.TryGetValue size with
        | true, page ->
            let atlasWidth = page.Width / size

            let glyphIdx = Convert.ToInt32(glyph);
            let glyphX = glyphIdx % atlasWidth * size;
            let glyphY = glyphIdx / atlasWidth * size;
            
            let cropper (g: IImageProcessingContext) =
                g.Crop(Rectangle(glyphX, glyphY, size, size))
                |> ignore

            page.Clone(cropper)
        | _ -> raise <| ArgumentOutOfRangeException(nameof(size), size, "not a known size")

let SegoeMdl2: Lazy<SymbolAtlas<Glyphs>> =
    lazy (
        let assembly = Assembly.GetExecutingAssembly()
        
        [| SymbolAtlasPage(getFileInBytes assembly "atlas16.png", 16)
           SymbolAtlasPage(getFileInBytes assembly "atlas32.png", 32)
           SymbolAtlasPage(getFileInBytes assembly "atlas64.png", 64) |]
        |> SymbolAtlas<Glyphs>
    )