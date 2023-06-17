export type State = {
    allGoals: Heights[];
    button: string;
    newGoalSection: NewGoalSectionInitialHeight;
    newGoal: boolean;
    goalDatas: GoalData[];
};

export type Heights = {
    initialHeight: number;
    finalHeight: number;
    initialMargin: number;
    finalMargin: number;
    state: string;
};

export type NewGoalSectionInitialHeight = {
    height?: number;
    width?: number;
};

export type GoalData = {
    title?: string;
    specific?: string;
    plan?: string;
    dueDate?: string;
};
