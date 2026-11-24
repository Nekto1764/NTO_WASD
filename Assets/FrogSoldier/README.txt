FrogSoldier — лягушка-пехотинец, риг + 3 анимации (30 fps, анимация на месте)

Клипы (внутри FrogSoldier.fbx, в Unity отображаются как "FrogSoldier|<имя>"):
  Walk   — кадры 0–24, 0,8 с, цикл (Loop Time = вкл.)
  Attack — кадры 0–24, 0,8 с, удар копьём; пик выпада ~кадр 10 (0,33 с) — сюда событие урона
  Death  — кадры 0–40, 1,33 с, без цикла; копьё выпадает из руки, тело падает на спину

Импорт в Unity:
  1. Скопировать папку FrogSoldier в Assets.
  2. FrogSoldier.fbx → Rig: Animation Type = Generic, Avatar = Create From This Model.
  3. Model: Bake Axis Conversion = вкл. (если модель повёрнута на -90° по X).
  4. Animation: у Walk включить Loop Time; Root Motion не использовать (Apply Root Motion = выкл.).
  5. Материал: Base Map = FrogSoldier_BaseColor, Normal Map = FrogSoldier_Normal (Texture Type = Normal map),
     Metallic Map = FrogSoldier_MetallicSmoothness (Source = Metallic Alpha). Occlusion — по желанию.

Масштаб: высота лягушки ~0,75 ед. (с копьём ~1,0). Персонаж смотрит вдоль +Z.
Скорость ходьбы без проскальзывания ног: ~0,33 ед./с при Animator speed = 1 (масштабируется вместе с моделью).

Кости: Root, Hips, Spine, Chest, Head, UpperArm/LowerArm/Hand (.L/.R), Thigh/Shin/Foot (.L/.R), Spear (дочерняя Hand.R).
FrogSoldier.glb — та же модель с анимациями (glTF), FrogSoldier.blend — исходник для правок.
