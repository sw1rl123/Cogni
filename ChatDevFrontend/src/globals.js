export const DEV = import.meta.env.DEV;
export const DOCKER_DEV = import.meta.env.VITE_DOCKER_DEV == "true";
console.log("ISDEV: ", DEV)
console.log("DOCKER_DEV: ", DOCKER_DEV)
export const apiBase = DOCKER_DEV ? "http://127.0.0.1:5108" : DEV ? "http://127.0.0.1:5108" : "/api"
export const fileApi =  DOCKER_DEV ? "http://212.22.82.127:9111" : DEV ? "http://127.0.0.1:9000" : ""
export const cogniApi =  DOCKER_DEV ? "http://127.0.0.1:5279" : DEV ? "http://127.0.0.1:5279" : "/api"

// uuid is Unique User Identifier now :D
let uuid_to_username = {}

export function addUsernameRelation(username, uuid) {
    uuid_to_username[uuid] = username;
}

export function getUsernameByUuid(uuid) {
    return uuid_to_username[uuid];
}

export function removeUsernameRelation(uuid) {
    delete uuid_to_username[uuid];
}

export function clearUsernameRelations() {
    uuid_to_username = {};
}


export async function fetchUsers(user_ids) {
    const response = await fetch(`${cogniApi}/user/GetUsersByIds`, {
        method: "POST",
        body: JSON.stringify(user_ids),
        headers: {
            "Content-Type": "application/json"
        }
    });

    if (response.ok) {
        const users = await response.json();
        clearUsernameRelations();

        for (const user of users) {
            addUsernameRelation(`${user.name} ${user.surname}`, user.id);
        }

        return users;
    } else {
        showToast("Failed to fetch users.");
    }
}

let currentUser = {
    userId: null
};

export function setCurrentUser(userId) {
    currentUser.userId = userId;
}

export function getCurrentUserId() {
    return currentUser.userId;
}

export function logout() {
    currentUser.userId = null;
}

export function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
}

export function showToast(message, color="rgba(255, 0, 0, 0.7)") {
    let toast = document.createElement("div");
    toast.textContent = message;
    toast.style.position = "fixed";
    toast.style.top = "20px";
    toast.style.left = "50%";
    toast.style.transform = "translateX(-50%)";
    toast.style.background = color;
    toast.style.color = "#fff";
    toast.style.padding = "10px 20px";
    toast.style.borderRadius = "5px";
    toast.style.zIndex = "1000";
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}