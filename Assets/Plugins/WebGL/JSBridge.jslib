mergeInto(LibraryManager.library, {

  // -------------------------
  // STRING MESSAGE
  // -------------------------
  SendStringMessage: function (messagePtr) {
    const message = UTF8ToString(messagePtr);

    console.log("[PolygonTD] String message:", message);

    window.dispatchEvent(new CustomEvent("PolygonTD-string-message", {
      detail: message
    }));
  },

  // -------------------------
  // LEVEL CLEARED
  // -------------------------
  SignalLevelCleared: function (levelNumber) {
    console.log("[PolygonTD] Level cleared:", levelNumber);

    window.dispatchEvent(new CustomEvent("PolygonTD-level-cleared", {
      detail: levelNumber
    }));
  },

  // -------------------------
  // LEVEL LOST
  // -------------------------
  SignalLevelLost: function (levelNumber) {
    console.log("[PolygonTD] Level lost:", levelNumber);

    window.dispatchEvent(new CustomEvent("PolygonTD-level-lost", {
      detail: levelNumber
    }));
  },

  // -------------------------
  // LEVEL STARTING
  // -------------------------
  SignalLevelStarting: function (levelNumber) {
    console.log("[PolygonTD] Level starting:", levelNumber);

    window.dispatchEvent(new CustomEvent("PolygonTD-level-starting", {
      detail: levelNumber
    }));
  },

  // -------------------------
  // PAUSE TOGGLED
  // -------------------------
  SignalPauseToggled: function (toggled) {
    const isPaused = toggled === 1;

    console.log("[PolygonTD] Pause toggled:", isPaused);

    window.dispatchEvent(new CustomEvent("PolygonTD-pause-toggled", {
      detail: isPaused
    }));
  },

  // -------------------------
  // SCENE CHANGE
  // -------------------------
  SignalSceneChange: function (scenePtr) {
    const sceneName = UTF8ToString(scenePtr);

    console.log("[PolygonTD] Scene change:", sceneName);

    window.dispatchEvent(new CustomEvent("PolygonTD-scene-change", {
      detail: sceneName
    }));
  },

  // -------------------------
  // PLAYER LEVEL PROGRESSION
  // -------------------------
  SignalPlayerLevelProgression: function (levelNumber) {
    console.log("[PolygonTD] Player level progression:", levelNumber);

    window.dispatchEvent(new CustomEvent("PolygonTD-player-level-progression", {
      detail: levelNumber
    }));
  },

});