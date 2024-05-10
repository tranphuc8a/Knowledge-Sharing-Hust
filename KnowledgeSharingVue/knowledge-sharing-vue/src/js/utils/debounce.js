

class Debounce{
    static debounce(callback, delay, options = {}){
        let timeoutId = null;
        const {leading = true, trailing = true} = options;

        return function(...args) {
            const context = this;
            
            if (leading && timeoutId == null) {
                callback.apply(context, args);
            }

            const invokeCallback = () => {
                timeoutId = null;
                if (trailing) {
                    callback.apply(context, args);
                }
            };

            clearTimeout(timeoutId);
            timeoutId = setTimeout(invokeCallback, delay);
        };
    }    


    static throttle(callback, deltaTime) {
        let shouldWait = false;
        let waitingArgs = null;
        
        const timeoutFunc = () => {
            if (waitingArgs == null) {
                shouldWait = false;
            } else {
                callback.apply(this, waitingArgs);
                waitingArgs = null;
                setTimeout(timeoutFunc, deltaTime);
            }
        };
        
        return function(...args) {
            if (shouldWait) {
                waitingArgs = args;
                return;
            }
        
            callback.apply(this, args);
            shouldWait = true;
        
            setTimeout(timeoutFunc, deltaTime);
        };
    }
}

export default Debounce;
