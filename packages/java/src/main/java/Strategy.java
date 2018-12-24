import model.*;

public interface Strategy {
    void act(Robot me, Rules rules, Game game, Action action);
    String customRendering();
}
